'''Solution to the 'Kolorowy Waz' Polish Informatics 2022 olympiad problem.'''
from typing import List
from flask import Flask, json, request, Response
from random import randrange

api = Flask(__name__)

@api.route("/tea")
def tea():
    return Response("", 418)

@api.route("/boardgen/<size_tmp>", methods=["GET"])
def gen_baord(size_tmp: str):
    '''Initializes the game with the specified map size and food data.'''
    # -1 means empty, food color otherwise, snake is not represented on the board
    size = int(size_tmp)
    board: List[List[int]] = [[-1 for x in range(size)] for y in range(size)]
    snake: List[List[int]] = [[0, 0]] # (y, x)
    snake_color_board: List[int] = [0] # 0 food color for elements in same order as snake
    board[randrange(1,size)][randrange(1,size)] = randrange(size)

    rtrn_data = {"board": board, "snake": snake, "snake_data": snake_color_board, "score": 0, "status": 0}
    return Response(json.dumps(rtrn_data), 200, mimetype='text/json')

@api.route("/move/<direction>", methods=["POST"])
def move(direction: str):
    '''Moves the snake in the specified direction, returns the color of the food eaten or None if none was eaten.'''
    data = request.get_json()
    if data['status'] != 0:
        data = {}
        data["RESPONSE"] = "INVALID STATUS: GAME ALREADY OVER."
        return Response(json.dumps(data), 400, mimetype="text/json")
    snake: List[List[int]] = data["snake"]
    head: List[int] = snake[0]
    board: List[List[int]] = data["board"]
    snake_data: List[int] = data["snake_data"]
    size = len(data["board"])
    proposed_coords = {
        'U': [head[0]-1, head[1]],
        'D': [head[0]+1, head[1]],
        'L': [head[0], head[1]-1],
        'R': [head[0], head[1]+1]
    }[direction]

    if (proposed_coords in snake) or (min(proposed_coords) < 0) or (max(proposed_coords) >= size):
        data['status'] = -1
        return Response(json.dumps(data), 200, mimetype="text/json")

    if board[proposed_coords[0]][proposed_coords[1]] != -1:
        data['score'] += 1
        if data["score"] >= (size**2)-1:
            data['status'] = 1
            return Response(json.dumps(data), 200, mimetype="text/json")
        snake.insert(0, proposed_coords)
        snake_data.insert(0, board[proposed_coords[0]][proposed_coords[1]])
        board[proposed_coords[0]][proposed_coords[1]] = -1
        while True:
            choice_loc = [randrange(size),randrange(size)]
            if choice_loc not in snake:
                break
        board[choice_loc[0]][choice_loc[1]] = randrange(size)
        return Response(json.dumps(data), 200, mimetype='text/json')

    snake.insert(0, proposed_coords)
    snake.pop()
    return Response(json.dumps(data), 200, mimetype='text/json')

if __name__ == '__main__':
    api.run(port=23500)