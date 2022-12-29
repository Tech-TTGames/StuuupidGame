'''Solution to the 'Kolorowy Waz' Polish Informatics 2022 olympiad problem.'''
from typing import List, Tuple
from flask import Flask, json, request
from random import randrange

api = Flask(__name__)

@api.route("/tea")
def tea():
    return "", 418

@api.route("/boardgen/<size>", methods=["GET"])
def gen_baord(size: str):
    '''Initializes the game with the specified map size and food data.'''
    # -1 means empty, food color otherwise, snake is not represented on the board
    size = int(size)
    board: List[List[int]] = [[-1 for x in range(size)] for y in range(size)]
    snake: List[Tuple[int,int]] = [(0, 0)] # (y, x)
    snake_color_board: List[int] = [0] # 0 food color for elements in same order as snake
    board[randrange(1,size)][randrange(1,size)] = randrange(size)

    rtrn_data = {"board": board, "snake": snake,"snake_color_board": snake_color_board, "score": 0}
    return json.dumps(rtrn_data), 200

@api.route("/move/<direction>", methods=["POST"])
def move(direction: str):
    '''Moves the snake in the specified direction, returns the color of the food eaten or None if none was eaten.'''
    data = request.get_json()
    snake: List[Tuple[int,int]] = data["snake"]
    head: Tuple = snake[0]
    board: List[List[int]] = data["board"]
    snake_color_board: List[int] = data["snake_color_board"]
    size = len(data["board"])
    proposed_coords = {
        'U': (head[0]-1, head[1]),
        'D': (head[0]+1, head[1]),
        'L': (head[0], head[1]-1),
        'R': (head[0], head[1]+1)
    }[direction]
    if board[proposed_coords[0]][proposed_coords[1]] != -1:
        board['score'] += 1
        snake.insert(0, (proposed_coords))
        snake_color_board.insert(0, board[proposed_coords[0]][proposed_coords[1]])
        board[proposed_coords[0]][proposed_coords[1]] = -1
        board[randrange(1,size)][randrange(1,size)] = randrange(size)
        return json.dumps(data), 200

    snake.insert(0, proposed_coords)
    snake.pop()
    return json.dumps(data), 200

if __name__ == '__main__':
    api.run(port=23500)