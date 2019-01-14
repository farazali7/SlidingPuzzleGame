using System;
using static System.Console;

namespace Bme121
{
    static class Program
    {
        // Play the 'double play' sliding puzzle game.
        // This game idea comes from Larry D. Nichols and is found at the
        // URL http://www.ageofpuzzles.com/Puzzles/DoublePlay/DoublePlay.htm.

        static void Main( )
        {
            Random rGen = new Random( );

            // Try to figure out which form of the board to display.
            // Non-Windows machines seem to not have the box-drawing characters.

            bool useU0000 = false;
            if( Environment.OSVersion.Platform == PlatformID.MacOSX ) useU0000 = true;
            if( Environment.OSVersion.Platform == PlatformID.Unix ) useU0000 = true;

            // Initialize the game board in the solved puzzle state.
            // The zero value represents a hole.

            int[ , ] board =
            {
                {  0,  1,  2,  0 },
                {  3,  4,  5,  6 },
                {  7,  8,  9, 10 },
                {  0, 11, 12,  0 }
            };
            
            

            // Dimensions of the game board are extracted into variables for convenience.

            int rows = board.GetLength( 0 );
            int cols = board.GetLength( 1 );
            int length = board.Length;
            

            // This is the main game-playing loop.
            // Each iteration is either performing one random move (as part of scrambling)
            // or one move entered by the user.

            bool quit = false;
            int randomMoves = 0;
            while( ! quit )
            {
                int move = 0;

                // Either generate a random move or display the game board and ask the user for a move.

                if( randomMoves > 0 )
                {
                    move = rGen.Next( 1, 13 );

                    randomMoves --;
                }
                else
                {
                    // Extract the game-board values into an array of displayed game-board strings.
                    // This is done so the strings can be of width 3 which makes the game-board
                    // display code below express very clearly.
					
                    string[ ] map = new string[ length ];
                    for( int i = 0; i < length; i ++ )
                    {
                        int value = board[ i / cols, i % cols ];
                        if( value == 0 ) map[ i ] = "   ";
                        else map[ i ] = $" {value:x} ";
                    }

                    // Display the game board.

                    Clear( );
                    WriteLine( );
                    WriteLine( " Welcome to the double-play game!" );
                    WriteLine( " Tiles slide in pairs by pushing towards a hole." );
                    WriteLine( " Scramble, then arrange back in order by sliding." );
                    WriteLine( );

                    if( useU0000 )
                    {
                        // Use Unicode 'C0 Controls and Basic Latin' range 0000–007f.

                        WriteLine( " +---+---+---+---+" );
                        WriteLine( " |{0}|{1}|{2}|{3}|", map[  0 ], map[  1 ], map[  2 ], map[  3 ] );
                        WriteLine( " +---+---+---+---+" );
                        WriteLine( " |{0}|{1}|{2}|{3}|", map[  4 ], map[  5 ], map[  6 ], map[  7 ] );
                        WriteLine( " +---+---+---+---+" );
                        WriteLine( " |{0}|{1}|{2}|{3}|", map[  8 ], map[  9 ], map[ 10 ], map[ 11 ] );
                        WriteLine( " +---+---+---+---+" );
                        WriteLine( " |{0}|{1}|{2}|{3}|", map[ 12 ], map[ 13 ], map[ 14 ], map[ 15 ] );
                        WriteLine( " +---+---+---+---+" );
                    }
                    else
                    {
                        // Use Unicode 'Box Drawing' range 2500–257f.

                        WriteLine( " ╔═══╦═══╦═══╦═══╗" );
                        WriteLine( " ║{0}║{1}║{2}║{3}║", map[  0 ], map[  1 ], map[  2 ], map[  3 ] );
                        WriteLine( " ╠═══╬═══╬═══╬═══╣" );
                        WriteLine( " ║{0}║{1}║{2}║{3}║", map[  4 ], map[  5 ], map[  6 ], map[  7 ] );
                        WriteLine( " ╠═══╬═══╬═══╬═══╣" );
                        WriteLine( " ║{0}║{1}║{2}║{3}║", map[  8 ], map[  9 ], map[ 10 ], map[ 11 ] );
                        WriteLine( " ╠═══╬═══╬═══╬═══╣" );
                        WriteLine( " ║{0}║{1}║{2}║{3}║", map[ 12 ], map[ 13 ], map[ 14 ], map[ 15 ] );
                        WriteLine( " ╚═══╩═══╩═══╩═══╝" );
                    }
                    WriteLine( );

                    // Interpret the user's desired move.

                    Write( " Tile to push (s to scramble, q to quit): " );
                    string response = ReadKey( intercept: true ).KeyChar.ToString( );
                    WriteLine( );

                    switch( response )
                    {
                        case "s": randomMoves = 100_000; break;

                        case "1": move =  1; break;
                        case "2": move =  2; break;
                        case "3": move =  3; break;
                        case "4": move =  4; break;
                        case "5": move =  5; break;
                        case "6": move =  6; break;
                        case "7": move =  7; break;
                        case "8": move =  8; break;
                        case "9": move =  9; break;
                        case "a": move = 10; break;
                        case "b": move = 11; break;
                        case "c": move = 12; break;

                        case "q": quit = true; break;
                    }
                }

                // If a move is possible, adjust the game board to make the move.

                if( move > 0 )
                {

                    // Initialize a boolean variable which will help end user's move.
                    bool endMove = false;
                    
                    // Find user's number by iterating through the board's rows first, then columns.
                    for (int rowLocation = 0; rowLocation < rows; rowLocation++)
                    {
						for (int columnLocation = 0; columnLocation < cols; columnLocation++)
						{
							
							// Once the user's number is located, check which row/column are in bounds at number's location.
							if (board[rowLocation, columnLocation] == move)
							{
								bool columnInRangeLeft = (columnLocation - 2) >= 0;
								bool columnInRangeRight = (columnLocation + 2) < cols;
								bool rowInRangeUp = (rowLocation - 2) >= 0;
								bool rowInRangeDown = (rowLocation + 2) < rows;

								// Set up boolean variables to later help ultimately declare if shift in certain direction is possible.
								bool shiftRight = false;
								bool shiftLeft = false;
								bool shiftUp = false;
								bool shiftDown = false;
								
								// If column to left is in bounds, check occupancy of spaces on left side.
								if (columnInRangeLeft)
								{
									bool spaceOnFarLeft = (board[rowLocation, columnLocation-2] == 0);
									bool numOnLeft = (board[rowLocation, columnLocation-1] != 0);
									
									if (spaceOnFarLeft && numOnLeft)
									{
										shiftLeft = true;
									}
								}
								
								// If column to right is in bounds, check occupancy of spaces on right side.
								if (columnInRangeRight)
								{
									bool spaceOnFarRight = (board[rowLocation, columnLocation+2] == 0);
									bool numOnRight = (board[rowLocation, columnLocation+1] != 0);
									
									if (spaceOnFarRight && numOnRight)
									{
										shiftRight = true;
									}
								}
								
								// If row above is in bounds, check occupancy of spaces above.
								if (rowInRangeUp)
								{
									bool spaceFarUp = (board[rowLocation-2, columnLocation] == 0);
									bool numUp = (board[rowLocation-1, columnLocation] != 0);
									
									if (spaceFarUp && numUp) 
									{
										shiftUp = true;
									}	
								}
								
								// If row below is in bounds, check occupancy of spaces below.
								if (rowInRangeDown)
								{
									bool spaceFarDown = (board[rowLocation+2, columnLocation] == 0);
									bool numDown = (board[rowLocation+1, columnLocation] != 0);
									
									if (spaceFarDown && numDown)
									{
										shiftDown = true;
									}
								}
								
								// Set to true to help break out of current for-loops and end user's move.
								endMove = true;
								
								// Check which shift is possible and apply it by rearranging board.
								if (shiftLeft)
								{
									board[rowLocation, columnLocation-2] = board[rowLocation, columnLocation-1];
									board[rowLocation, columnLocation-1] = move;
									board[rowLocation, columnLocation] = 0;
								}
								else if (shiftRight)
								{
									board[rowLocation, columnLocation+2] = board[rowLocation, columnLocation+1];
									board[rowLocation, columnLocation+1] = move;
									board[rowLocation, columnLocation] = 0;
								}
								else if (shiftUp)
								{
									board[rowLocation-2, columnLocation] = board[rowLocation-1, columnLocation];
									board[rowLocation-1, columnLocation] = move;
									board[rowLocation, columnLocation] = 0;
								}
								else if (shiftDown)
								{
									board[rowLocation+2, columnLocation] = board[rowLocation+1, columnLocation];
									board[rowLocation+1, columnLocation] = move;
									board[rowLocation, columnLocation] = 0;
								}
								
								break;
									
							}
						}
						
						// Break out of current iterations and wait for user's next input.
						if (endMove)
						{
							break;
						}						
					}
                }
            }

            
			WriteLine( " Thanks for playing!" );
            WriteLine( ); 
        }
    }
}

//~ Feedback:

// Program works as expected and is well formatted!
// Too many variables were declared, if you are only going to use a variable once in your whole program
//	perhaps it is not necessary (e.x., spaceOnFarRight), and a comment should be used to explain
//  the code instead of a variable name.
//	
// Some variable names are a bit too long (i.e. columnLocation could just be col).
//	 Try to keep variable names as concise as possible.
 
//~ For marking:

//~ 1 mark: given "move", a tile, the code correctly determines the row and column of the tile on the board

//~ 1 mark: the code checks and correctly determines which of the 4 directions the chosen tile can move

//~ 1 mark: the code correctly performs the "move" by manipulating the board values to slide a pair of tiles, for at least one of the directions

//~ 1 mark: the code correctly performs moves in all 4 directions, when issued individually and when using the scramble command

//~ 1 mark: the code is well structured, documented with comments, and formatted


// Final Mark: 5/5
