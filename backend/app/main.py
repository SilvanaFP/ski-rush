from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from passlib.context import CryptContext
import psycopg2

from app.database import get_connection

app = FastAPI(title="Ski Rush API")

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")


class UserAuth(BaseModel):
    username: str
    password: str


class ScoreCreate(BaseModel):
    user_id: int
    score: int


@app.get("/")
def read_root():
    return {"message": "Ski Rush API funcionant"}


@app.get("/db-test")
def db_test():
    conn = get_connection()
    cur = conn.cursor()

    cur.execute("SELECT NOW() AS current_time;")
    result = cur.fetchone()

    cur.close()
    conn.close()

    return {
        "message": "Connexió amb PostgreSQL correcta",
        "database_time": result["current_time"]
    }


@app.post("/register")
def register(user: UserAuth):
    username = user.username.strip()
    password = user.password.strip()

    if username == "" or password == "":
        raise HTTPException(
            status_code=400,
            detail="El nom d'usuari i la contrasenya són obligatoris"
        )

    password_hash = pwd_context.hash(password)

    conn = get_connection()
    cur = conn.cursor()

    try:
        cur.execute(
            """
            INSERT INTO users (username, password_hash)
            VALUES (%s, %s)
            RETURNING id, username;
            """,
            (username, password_hash)
        )

        new_user = cur.fetchone()
        conn.commit()

        return {
            "id": new_user["id"],
            "username": new_user["username"]
        }

    except psycopg2.errors.UniqueViolation:
        conn.rollback()
        raise HTTPException(
            status_code=400,
            detail="User already exists"
        )

    finally:
        cur.close()
        conn.close()


@app.post("/login")
def login(user: UserAuth):
    username = user.username.strip()
    password = user.password.strip()

    conn = get_connection()
    cur = conn.cursor()

    cur.execute(
        """
        SELECT id, username, password_hash
        FROM users
        WHERE username = %s;
        """,
        (username,)
    )

    db_user = cur.fetchone()

    cur.close()
    conn.close()

    if db_user is None:
        raise HTTPException(
            status_code=401,
            detail="User or password incorrect"
        )

    password_correcta = pwd_context.verify(
        password,
        db_user["password_hash"]
    )

    if not password_correcta:
        raise HTTPException(
            status_code=401,
            detail="User or password incorrect"
        )

    return {
        "id": db_user["id"],
        "username": db_user["username"]
    }


@app.post("/scores")
def save_score(score_data: ScoreCreate):
    if score_data.score < 0:
        raise HTTPException(
            status_code=400,
            detail="La puntuació no pot ser negativa"
        )

    conn = get_connection()
    cur = conn.cursor()

    try:
        cur.execute(
            """
            INSERT INTO scores (user_id, score)
            VALUES (%s, %s)
            RETURNING id, user_id, score, created_at;
            """,
            (score_data.user_id, score_data.score)
        )

        new_score = cur.fetchone()
        conn.commit()

        return {
            "id": new_score["id"],
            "user_id": new_score["user_id"],
            "score": new_score["score"],
            "created_at": new_score["created_at"]
        }

    except psycopg2.errors.ForeignKeyViolation:
        conn.rollback()
        raise HTTPException(
            status_code=400,
            detail="User does not exist"
        )

    finally:
        cur.close()
        conn.close()


@app.get("/ranking")
def get_ranking():
    conn = get_connection()
    cur = conn.cursor()

    cur.execute(
        """
        SELECT 
            u.username,
            s.score,
            TO_CHAR(s.created_at, 'DD/MM/YYYY HH24:MI') AS created_at
        FROM scores s
        INNER JOIN users u ON s.user_id = u.id
        ORDER BY s.score DESC, s.created_at ASC
        LIMIT 5;
        """
    )

    ranking = cur.fetchall()

    cur.close()
    conn.close()

    result = []

    for index, row in enumerate(ranking):
        result.append({
            "position": index + 1,
            "username": row["username"],
            "score": row["score"],
            "created_at": row["created_at"]
        })

    return result


@app.get("/users/{username}/scores")
def get_user_scores(username: str):
    conn = get_connection()
    cur = conn.cursor()

    cur.execute(
        """
        SELECT 
            u.username,
            s.score,
            TO_CHAR(s.created_at, 'DD/MM/YYYY HH24:MI') AS created_at
        FROM scores s
        INNER JOIN users u ON s.user_id = u.id
        WHERE u.username = %s
        ORDER BY s.created_at DESC;
        """,
        (username,)
    )

    scores = cur.fetchall()

    cur.close()
    conn.close()

    return scores