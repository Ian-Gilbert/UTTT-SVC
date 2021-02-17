# Ultimate Tic Tic Toe

[![build status](https://github.com/Ultimate-Tic-Tac-Toe/uttt-svc/workflows/build/badge.svg)](https://github.com/Ultimate-Tic-Tac-Toe/uttt-svc/actions?query=workflow%3Abuild)
[![release status](https://github.com/Ultimate-Tic-Tac-Toe/uttt-svc/workflows/release/badge.svg)](https://github.com/Ultimate-Tic-Tac-Toe/uttt-svc/actions?query=workflow%3Arelease)
[![coverage status](https://sonarcloud.io/api/project_badges/measure?project=uttt-svc&metric=coverage)](https://sonarcloud.io/dashboard?id=uttt-svc)
[![maintainability rating](https://sonarcloud.io/api/project_badges/measure?project=uttt-svc&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=uttt-svc)
[![reliability rating](https://sonarcloud.io/api/project_badges/measure?project=uttt-svc&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=uttt-svc)
[![security rating](https://sonarcloud.io/api/project_badges/measure?project=uttt-svc&metric=security_rating)](https://sonarcloud.io/dashboard?id=uttt-svc)

This is a Rest API for the game of Ultimate Tic Tac Toe, with both two-player and human vs bot capabilities.

## Endpoints

### GET /rest/uttt/uttt/{id}
Return a game by its ID number.

### POST /rest/uttt/uttt
Create a new game.

### PUT /rest/uttt/uttt/{id}
Make a move on the game with the given ID number. Body contains player/move info.

### DELETE /rest/uttt/uttt/{id}
Delete a game by its ID number.
