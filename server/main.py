from fastapi import FastAPI
from fastapi.staticfiles import StaticFiles
from fastapi.middleware.cors import CORSMiddleware
import uvicorn
from utils.environment import DEVELOPMENT, PORT
from endpoints import sockets, api


app = FastAPI()

if DEVELOPMENT:
    app.add_middleware(
        CORSMiddleware,
        allow_origins=["*"],
        allow_credentials=True,
        allow_methods=["*"],
        allow_headers=["*"],
    )

app.include_router(sockets)
app.include_router(api)

if not DEVELOPMENT:
    app.mount("/", StaticFiles(directory="client/build", html=True), name="static")

if __name__ == "__main__":
    uvicorn.run(
        "main:app",
        host="0.0.0.0",
        port=PORT,
        reload=DEVELOPMENT,
    )
