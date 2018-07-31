# mswep

## A console clone of the classic Minesweeper

An endeavour that started while waiting on a meeting with a professor, I am recreating one of my favorite simple games as a console application to tune as I please.
I have minesweeper on my phone, but wanted to experience the same joy I do playing it with my thumbs in the console.

Here is mswep, a condensed edition of a old favorite.

---

### Instructions

The first prompt upon entering the game from the menu is a size. The grid generated will measure a corresponding number of cells on each side.
Although the dimensions are technically square in the number of cells present, the player's console font and sizing may distort this square.
Monospace fonts should render the UI in a readable format.

After selecting a size, the grid will be drawn, including the number of mines placed in the grid and the number remaining, as well as guides along the axes for identifying the cells.
The number of remaining cells is simply provided as an estimate based on the cells the player has marked.

To uncover a given cell, enter its coordinates according to the guides in the prompt using the given format: `X.Y`

If the player believes a cell contains a mine, they can mark it with a star for identification later, and to prevent accidentally uncovering that cell using the given format: `*X.Y`

(Note: Markings can be removed by repeating this process on a given cell)

The game will end in victory when all mines are marked, or by isolating the mines when all empty cells are uncovered.

Uncovering a mine will end the game with an immediate Loss.

A shortened version of these instructions is available from the in-game menu by selecting `I`

---

### Development

The randomization and upper limit on mines is still in progress, but the current settings seem to land about medium difficulty in my experience. If anyone plays around with this and finds a better set of parameters, let me know. I'm not too concerned with it because it's playable now and this is not a main project for me. I will continue to tweak and update as I have time.

All feedback is appreciated, and I will respond, so feel free.  
