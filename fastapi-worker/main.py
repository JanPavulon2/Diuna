from fastapi import FastAPI, Body

app = FastAPI()

@app.get("/hello")
async def hello_world():
    return {"message": "Hello, World from FastAPI!"}

@app.get("/greet/{name}")
async def greet(name: str, age: int = None):
    if age:
        return {"message": f"Hello, {name}. You are {age} years old."}
    return {"message": f"Hello, {name}. Your age is unknown."}

@app.post("/submit")
async def submit_data(data: dict = Body(...)):
    return {"received": data}

# Example of returning a custom status code
@app.get("/status", status_code=201)
async def return_status():
    return {"message": "This is a custom status code"}


# print("JJJJJJJJPPPPPPPPPPP22222222222222GGGGGGGGGGGGMMMMMMMMMDDDDDD");