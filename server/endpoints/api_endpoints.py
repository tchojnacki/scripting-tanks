from fastapi import APIRouter

api = APIRouter(prefix="/api")


@api.get("/hello")
def service():
    return {"message": "Hello world!"}
