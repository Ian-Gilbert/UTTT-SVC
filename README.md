# Ultimate Tic Tic Toe

This is a Rest API for the game of Ultimate Tic Tac Toe, taking over from my [older python version of the game](https://github.com/Ian-Gilbert/Ultimate-Tic-Tac-Toe).

## How To Play
On the surface, ultimate tic tac toe is the same as the standard game: there are nine squares arranged in a 3x3 grid which can each be marked as either an 'X' or an 'O', and three in a row wins the game. Unlike the original game, however, you cannot simply mark a square as 'X' or 'O'. Instead, each square consists of an additional tic tac toe game, which must be won in order to mark the big square. The overall board is called the 'global board', and the smaller boards are called 'local boards'. The game is won when a player wins three local boards in a row.

On the first turn, Player 'X' can choose any square in any local board they like. From then on, however, the next move will be determined in part by the previous player's move. For example, if Player 'X' plays in the bottom left square of their local board, Player 'O' must then make their next move somewhere in the bottom left local board. This will then determine which local board Player 'X' must play in, and so on. This creates interesting situations in which you may purposefully not win a local board for fear of placing your opponent in an even better position.

**_Important Note:_** Each local board can be won, lost, or drawn (meaning every space is used up, there is no forecasting a draw), at which point no more moves may be made on that board. If a player is sent to such a board, they may then play in any open board they choose (this is something to avoid). Other implementations of the game allow you to play in a local board that has already been won, however this leads to an unbeatable strategy, as described in [this video](https://www.youtube.com/watch?v=weC1pAeh2Do).

For more information about the game, check out [this link](https://mathwithbaddrawings.com/2013/06/16/ultimate-tic-tac-toe/).

## Bot Options
This game features multiple difficulty levels of AI to play against. These are outlined below:

### Minimax (Beginner Difficulty)
The easiest bot uses the very well-known minimax algorithm to solve a standard tic tac toe board. For each move, the bot checks if the next local board has been determined. If not, it uses the minimax algorithm to decide which board to play on. Once it has the board, it again uses the minimax algorithm to choose its space on the board.

The minimax algorithm works very well for standard tic tac toe, where the entire game is solved very quickly and a simple heuristic function (reward/punishment for win, lose, draw) can be applied. This is not so easy for ultimate tic tac toe, because the high branching factor makes the algorithm take an unreasonably long time to complete. This is usually solved by using a heuristic function that attempts to estimate the value of a position without needing to reach a leaf node, however this bot bypasses the issue entirely by only performing the algorithm on isolated standard boards. As a result, it cannot make strategic descisions as to where it wants to send or avoid sending its opponent. It can also make very predictable moves, which makes it relatively easy to exploit by anticipating only a move or two in advance. This is a necessary skill in order to improve in ultimate tic tac toe, and so this is an ideal entry level bot.

### Monte Carlo Tree Search (Variable Difficulty)
The Monte Carlo Tree Search is an algorithm that efficiently searches through the game tree for the best move. Like minimax, there is no way to run through the entire tree in a reasonable amount of time, so instead it holds the current best move based on the branches checked so far, and updates it as it searches through the tree. This means that we can vary the difficulty of the bot by adjusting the time it has to find the best move. The shorter the limit, the worse a guess it will be.

## Endpoints

### GET /rest/uttt/{id}
Return a game by its ID number.

### POST /rest/uttt
Create a new game.

### PUT /rest/uttt/{id}
Make a move on the game with the given ID number. Body contains player/move info.

### DELETE /rest/uttt/{id}
Delete a game by its ID number.
