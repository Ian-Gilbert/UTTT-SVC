# Ultimate Tic Tic Toe

This is a Rest API for the game of Ultimate Tic Tac Toe, with both two-player and human vs bot capabilities.

## Endpoints

### GET /rest/uttt/{id}
Return a game by its ID number.

### POST /rest/uttt
Create a new game.

### PUT /rest/uttt/{id}
Make a move on the game with the given ID number. Body contains player/move info.

### DELETE /rest/uttt/{id}
Delete a game by its ID number.
