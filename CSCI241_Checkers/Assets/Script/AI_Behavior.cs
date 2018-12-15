﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AI_Behavior : CheckersBoard
{
    //Define the offensive points 
    readonly int CAPTUREPIECE = 2;
    readonly int CAPTUREKING = 4;
    readonly int CAPTUREDOUBLE = 5;
    readonly int MAKEKING = 4;
    readonly int OPPATRISK = 1;
    readonly int OPPKINGATRISK = 3;


    //Define defensive points 
    readonly int ATRISK = -1;
    readonly int KINGATRISK = -3;
    readonly int KINGDEAD = -4;
    readonly int LOSTPIECE = -2;
    readonly int MAKEOPPKING = -3;





    //Function that copies the original board and creates a copy in Fpiece
    public FPiece[,] CopyMainBoard(Piece[,] board)
    {
        //New board to store the result in
        FPiece[,] result = new FPiece[8, 8];


        //Initialize the new board
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                result[i, j] = null;
            }
        }


        //Copy everything from the old board onto new board
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x, y] != null)
                {

                    FPiece current = new FPiece
                    {
                        x = board[x, y].x,
                        y = board[x, y].y,
                        isKing = board[x, y].isKing,
                        isWhite = board[x, y].isWhite
                    };
                    result[x, y] = current;

                }
            }
        }

        return result;
    }





    //Function that creates a copy of FPiece[,]
    public FPiece[,] CopyBoard(FPiece[,] board)
    //This function was necessary along with the first one to make virtual moves possible
    {
        //New board to copy onto
        FPiece[,] result = new FPiece[8, 8];


        //Initialize new board
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                result[i, j] = null;
            }
        }


        //Copy everything from the old board onto new board
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x, y] != null)
                {

                    FPiece current = new FPiece
                    {
                        x = board[x, y].x,
                        y = board[x, y].y,
                        isKing = board[x, y].isKing,
                        isWhite = board[x, y].isWhite
                    };
                    result[x, y] = current;

                }
            }
        }


        return result;
    }






    //Function that returns all the black pieces on the board as a list
    public List<FPiece> GetBlackPieces(FPiece[,] board)
    {
        //Resultant list to add the black pieces of the board onto
        List<FPiece> result = new List<FPiece>();


        //Loop through the board and find all the black pieces
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if ((board[x, y] != null) && (board[x, y].isWhite == false))
                {
                    result.Add(board[x, y]);
                }
            }
        }

        //Return the list
        return result;
    }






    //Function that returns all the white pieces on the board as a list
    public List<FPiece> GetWhitePieces(FPiece[,] board)
    {
        //List onto which all the white pieces of the board is stored
        List<FPiece> result = new List<FPiece>();


        //Loop through the board and find all the white pieces
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if ((board[x, y] != null) && (board[x, y].isWhite))
                {
                    result.Add(board[x, y]);
                }
            }
        }


        //Return the resultant list
        return result;
    }







    //Function to get a list of all the possible moves in the passed in board state for the piece 'p'
    private List<FPiece> GetPossibleMoves(FPiece p, FPiece[,] board)
    {

        //Resultant list of faux pieces onto which the destinations will be saved
        List<FPiece> result = new List<FPiece>();


        //Check if the movement of the passed-in piece has been restricted
        if (p.IsForceMovement(board, p.x, p.y))
        {

            //If the piece is a king. 
            if (p.isKing)
            {

                //Check the four corners around it

                //Bottom right
                if (p.CheckMoveValidation(board, p.x, p.y, p.x + 2, p.y - 2))
                {
                    FPiece n = new FPiece(p.x + 2, p.y - 2);
                    result.Add(n);
                }

                //Bottom left 
                if (p.CheckMoveValidation(board, p.x, p.y, p.x - 2, p.y - 2))
                {
                    FPiece n = new FPiece(p.x - 2, p.y - 2);
                    result.Add(n);
                }

                //Top right
                if (p.CheckMoveValidation(board, p.x, p.y, p.x + 2, p.y + 2))
                {
                    FPiece n = new FPiece(p.x + 2, p.y + 2);
                    result.Add(n);
                }

                //Top left 
                if (p.CheckMoveValidation(board, p.x, p.y, p.x - 2, p.y + 2))
                {
                    FPiece n = new FPiece(p.x - 2, p.y + 2);
                    result.Add(n);
                }

                //Checked all possible places for king, return list
                return result;
            }

            //Else if its not a king but is white 
            if (p.isWhite)
            {
                //Check top right corner
                if (p.CheckMoveValidation(board, p.x, p.y, p.x + 2, p.y + 2))
                {
                    FPiece n = new FPiece(p.x + 2, p.y + 2);
                    result.Add(n);
                }

                //Check top left corner
                if (p.CheckMoveValidation(board, p.x, p.y, p.x - 2, p.y + 2))
                {
                    FPiece n = new FPiece(p.x - 2, p.y + 2);
                    result.Add(n);
                }

                //All possible locations checked. Return result.
                return result;
            }


            //Finally, if it's black
            if (!p.isWhite)
            {
                //Check bottom right corner
                if (p.CheckMoveValidation(board, p.x, p.y, p.x + 2, p.y - 2))
                {
                    FPiece n = new FPiece(p.x + 2, p.y - 2);
                    result.Add(n);
                }

                //Check bottom left corner
                if (p.CheckMoveValidation(board, p.x, p.y, p.x - 2, p.y - 2))
                {
                    FPiece n = new FPiece(p.x - 2, p.y - 2);
                    result.Add(n);
                }

                //All possible positions checked, return result.
                return result;
            }

        }


        //Else, if the movement isn't forced. Check for three cases : King, white, black
        else
        {

            //If its NOT forced movement and is king
            if (p.isKing)
            {
                //Checking bottom left
                if (p.CheckMoveValidation(board, p.x, p.y, p.x - 1, p.y - 1))
                {
                    FPiece n = new FPiece(p.x - 1, p.y - 1);
                    result.Add(n);
                }

                //Checking top left
                if (p.CheckMoveValidation(board, p.x, p.y, p.x - 1, p.y + 1))
                {
                    FPiece n = new FPiece(p.x - 1, p.y + 1);
                    result.Add(n);
                }

                //checking top right
                if (p.CheckMoveValidation(board, p.x, p.y, p.x + 1, p.y + 1))
                {
                    FPiece n = new FPiece(p.x + 1, p.y + 1);
                    result.Add(n);
                }

                //checking bottom right
                if (p.CheckMoveValidation(board, p.x, p.y, p.x + 1, p.y - 1))
                {
                    FPiece n = new FPiece(p.x + 1, p.y - 1);
                    result.Add(n);
                }

                return result;
            }


            //If p is not the king, then need to check for white and black

            //If its white
            if (p.isWhite)
            {
                //Check top left
                if (p.CheckMoveValidation(board, p.x, p.y, p.x - 1, p.y + 1))
                {
                    FPiece n = new FPiece(p.x - 1, p.y + 1);
                    result.Add(n);
                }

                //Check top right
                if (p.CheckMoveValidation(board, p.x, p.y, p.x + 1, p.y + 1))
                {
                    FPiece n = new FPiece(p.x + 1, p.y + 1);
                    result.Add(n);
                }

                //All places checked, return result
                return result;
            }

            if (!p.isWhite)
            {
                //if the piece is black

                //Check bottom left
                if (p.CheckMoveValidation(board, p.x, p.y, p.x - 1, p.y - 1))
                {
                    if (board[p.x - 1, p.y - 1] == null)
                    {
                        FPiece n = new FPiece(p.x - 1, p.y - 1);
                        result.Add(n);
                    }
                }

                //Check bottom right
                if (p.CheckMoveValidation(board, p.x, p.y, p.x + 1, p.y - 1))
                {
                    if (board[p.x + 1, p.y - 1] == null)
                    {
                        FPiece n = new FPiece(p.x + 1, p.y - 1);
                        result.Add(n);
                    }
                }

                //All possible places checked, return result
                return result;
            }
        }

        //If it reaches here, there aren't any possible moves. Return empty list
        return result;
    }





    //Function to make a virtual move on a virtual board 
    private Move MakeVirtualMove(FPiece source, FPiece dest, Move thisMove)
    {
        //Variable to store the score colleceted by the virtual move made
        int newScore = 0;

        //Store the king status of the passes in piece
        bool wasKing;
        if (source.isKing)
        {
            wasKing = true;
        }
        else
        {
            wasKing = false;
        }

        //Make a copy of the board that has been passed in
        FPiece[,] newBoard = CopyBoard(thisMove.GetBoard());

        //Check if the move is valid 
        if (source.CheckMoveValidation(newBoard, source.x, source.y, dest.x, dest.y))
        //If the move is valid, carry it out in the virtual board
        {


            //source co-ordiantes
            int x1 = source.x;
            int y1 = source.y;
            //dest coordinates
            int x2 = dest.x;
            int y2 = dest.y;

            //Check if the move is a killmove 
            if (Mathf.Abs(x1 - x2) == 2)
            {
                FPiece dead = newBoard[((x1 + x2) / 2), ((y1 + y2) / 2)]; //position of dead piece
                //Check if you actually kill something
                if (dead != null)
                {
                    newBoard[((x1 + x2) / 2), ((y1 + y2) / 2)] = null;

                    //Check if the king was killed and implement score correspondingly
                    if (dead.isKing)
                    {
                        //Check if AI killed white
                        if (dead.isWhite)
                        {
                            newScore = newScore + CAPTUREKING;
                        }
                        else
                        {
                            //IF AIKING was killed
                            newScore = newScore + KINGDEAD;
                        }

                    }
                    else
                    //If the king wasn't killed, a regular piece was
                    {
                        if (dead.isWhite)
                        {
                            //IF AI piece killed white piece
                            newScore = newScore + CAPTUREPIECE;
                        }
                        else
                        {
                            //IF AIpiece was killed by white
                            newScore = newScore + LOSTPIECE;
                        }
                    }
                }
                else
                //If the function reaches here, there was some error. Send out debug message and return
                {
                    Debug.Log("Error in Make virtual move > Kill move.");
                    return thisMove;
                }
            }


            //Create a new faux piece that is a duplicate of the source piece to move
            FPiece newSource = new FPiece(source.x, source.y, source.isWhite, source.isKing);

            //Move the piece onto the new board
            newBoard[x2, y2] = newSource;
            newBoard[x1, y1] = null;

            //Update the x and y vaules of the new piece
            newSource.x = x2;
            newSource.y = y2;

            //check to see if the source piece needs to be turned into a king 
            //check for white piece 
            if ((newSource.isWhite) && (y2 == 7))
            {
                newSource.isKing = true;
            }
            //check for black piece
            else if ((!newSource.isWhite) && (y2 == 0))
            {
                newSource.isKing = true;
            }

            //If the newSource wasn't king before, but is now, append score 
            if (!wasKing && newSource.isKing)
            {

                if (!newSource.isWhite)
                {
                    newScore = newScore + MAKEKING;
                }
                else
                {
                    newScore = newScore + MAKEOPPKING;
                }
            }



            //Now do the rest of Score checking for the current board status starting here: TODO
            //REMEMBER AI IS BLACK IN COLOR so only take into account the score for black color. 
            //NOW ALL WE NEED TO DO IS CALCULATE "AT-RISK" FACTORS.
            //if the piece that made a move is a while piece, check to see if it can be taken by black pieces
            if (newSource.isWhite)
            {
                // check index out of bounds
                if (!(((x2 + 1) < 0) || ((x2 + 1) > 7) || ((y2 + 1) < 0) || ((y2 + 1) > 7)) && (newBoard[x2 + 1, y2 + 1] != null))
                {
                    //if there's a black piece to the top right and an empty spot to the bottom left, it can be taken
                    if (!newBoard[x2 + 1, y2 + 1].isWhite)
                    {
                        // check index out of bounds for bottom left
                        if (!(((x2 - 1) < 0) || ((x2 - 1) > 7) || ((y2 - 1) < 0) || ((y2 - 1) > 7)))
                        {
                            if (newBoard[x2 - 1, y2 - 1] == null)
                            {
                                if (newSource.isKing)
                                {
                                    newScore = newScore + OPPKINGATRISK;
                                }
                                else
                                {
                                    newScore = newScore + OPPATRISK;
                                }
                            }
                        }

                    }
                }

                // check index out of bounds
                if (!(((x2 - 1) < 0) || ((x2 - 1) > 7) || ((y2 + 1) < 0) || ((y2 + 1) > 7)) && (newBoard[x2 - 1, y2 + 1] != null))
                {
                    //if there's a black piece to the top left and an empty spot to the bottom right, it can be taken
                    if (!newBoard[x2 - 1, y2 + 1].isWhite)
                    {
                        // check index out of bounds
                        if (!(((x2 + 1) < 0) || ((x2 + 1) > 7) || ((y2 - 1) < 0) || ((y2 - 1) > 7)))
                        {
                            if (newBoard[x2 + 1, y2 - 1] == null)
                            {
                                if (newSource.isKing)
                                {
                                    newScore = newScore + OPPKINGATRISK;
                                }
                                else
                                {
                                    newScore = newScore + OPPATRISK;
                                }
                            }
                        }
                    }
                }

                // check index out of bounds
                if (!(((x2 - 1) < 0) || ((x2 - 1) > 7) || ((y2 - 1) < 0) || ((y2 - 1) > 7)) && (newBoard[x2 - 1, y2 - 1] != null))
                {
                    //if there's a black king to the bottom left and an empty spot to the top right, it can be taken
                    if (!newBoard[x2 - 1, y2 - 1].isWhite && newBoard[x2 - 1, y2 - 1].isKing)
                    {
                        // check index out of bounds
                        if (!(((x2 + 1) < 0) || ((x2 + 1) > 7) || ((y2 + 1) < 0) || ((y2 + 1) > 7)))
                        {
                            if (newBoard[x2 + 1, y2 + 1] == null)
                            {
                                if (newSource.isKing)
                                {
                                    newScore = newScore + OPPKINGATRISK;
                                }
                                else
                                {
                                    newScore = newScore + OPPATRISK;
                                }
                            }
                        }
                    }
                }

                // check index out of bounds
                if (!(((x2 + 1) < 0) || ((x2 + 1) > 7) || ((y2 - 1) < 0) || ((y2 - 1) > 7)) && (newBoard[x2 + 1, y2 - 1] != null))
                {
                    //if there's a black king to the bottom right and an empty spot to the top left, it can be taken
                    if (!newBoard[x2 + 1, y2 - 1].isWhite && newBoard[x2 + 1, y2 - 1].isKing)
                    {
                        // check index out of bounds
                        if (!(((x2 - 1) < 0) || ((x2 - 1) > 7) || ((y2 + 1) < 0) || ((y2 + 1) > 7)))
                        {
                            if (newBoard[x2 - 1, y2 + 1] == null)
                            {
                                if (newSource.isKing)
                                {
                                    newScore = newScore + OPPKINGATRISK;
                                }
                                else
                                {
                                    newScore = newScore + OPPATRISK;
                                }
                            }
                        }
                    }
                }
            }

            //if the piece  that made a move is a black piece, check to see if it can be taken by white pieces
            if (!newSource.isWhite)
            {
                // check index out of bounds
                if (!(((x2 + 1) < 0) || ((x2 + 1) > 7) || ((y2 - 1) < 0) || ((y2 - 1) > 7)) && (newBoard[x2 + 1, y2 - 1] != null))
                {
                    //if there's a white piece to the bottom right and an empty spot to the top left, it can be taken
                    if (newBoard[x2 + 1, y2 - 1].isWhite)
                    {
                        // check index out of bounds
                        if (!(((x2 - 1) < 0) || ((x2 - 1) > 7) || ((y2 + 1) < 0) || ((y2 + 1) > 7)))
                        {
                            if (newBoard[x2 - 1, y2 + 1] == null)
                            {
                                if (newSource.isKing)
                                {
                                    newScore = newScore + KINGATRISK;
                                }
                                else
                                {
                                    newScore = newScore + ATRISK;
                                }
                            }
                        }

                    }
                }

                // check index out of bounds
                if (!(((x2 - 1) < 0) || ((x2 - 1) > 7) || ((y2 - 1) < 0) || ((y2 - 1) > 7)) && (newBoard[x2 - 1, y2 - 1] != null))
                {
                    //if there's a white piece to the bottom left and an empty spot to the top right, it can be taken
                    if (newBoard[x2 - 1, y2 - 1].isWhite)
                    {
                        // check index out of bounds
                        if (!(((x2 + 1) < 0) || ((x2 + 1) > 7) || ((y2 + 1) < 0) || ((y2 + 1) > 7)))
                        {
                            if (newBoard[x2 + 1, y2 + 1] == null)
                            {
                                if (newSource.isKing)
                                {
                                    newScore = newScore + KINGATRISK;
                                }
                                else
                                {
                                    newScore = newScore + ATRISK;
                                }
                            }
                        }
                    }
                }

                // check index out of bounds
                if (!(((x2 - 1) < 0) || ((x2 - 1) > 7) || ((y2 + 1) < 0) || ((y2 + 1) > 7)) && (newBoard[x2 - 1, y2 + 1] != null))
                {
                    //if there's a white king to the top left and an empty spot to the bottom right, it can be taken
                    if (newBoard[x2 - 1, y2 + 1].isWhite && newBoard[x2 - 1, y2 + 1].isKing)
                    {
                        // check index out of bounds
                        if (!(((x2 + 1) < 0) || ((x2 + 1) > 7) || ((y2 - 1) < 0) || ((y2 - 1) > 7)))
                        {
                            if (newBoard[x2 + 1, y2 - 1] == null)
                            {
                                if (newSource.isKing)
                                {
                                    newScore = newScore + KINGATRISK;
                                }
                                else
                                {
                                    newScore = newScore + ATRISK;
                                }
                            }
                        }
                    }
                }

                // check index out of bounds
                if (!(((x2 + 1) < 0) || ((x2 + 1) > 7) || ((y2 + 1) < 0) || ((y2 + 1) > 7)) && (newBoard[x2 + 1, y2 + 1] != null))
                {
                    //if there's a white king to the top right and an empty spot to the bottom left, it can be taken
                    if (newBoard[x2 + 1, dest.y + 1].isWhite && newBoard[x2 + 1, y2 + 1].isKing)
                    {
                        // check index out of bounds
                        if (!(((x2 - 1) < 0) || ((x2 - 1) > 7) || ((y2 - 1) < 0) || ((y2 - 1) > 7)))
                        {
                            if (newBoard[x2 - 1, y2 - 1] == null)
                            {
                                if (newSource.isKing)
                                {
                                    newScore = newScore + KINGATRISK;
                                }
                                else
                                {
                                    newScore = newScore + ATRISK;
                                }
                            }
                        }
                    }
                }
            }

        }







        //print(newScore);

        //Finally, create a new Move with the score for this movement, the new board and the original Starting moves
        Move result = new Move(thisMove.GetOrigin(), thisMove.GetDestination(), newBoard);
        result.AddScore(newScore);

        //Return the move
        return result;
    }






    List<Move> finalList = new List<Move>(); //global variable to store the end board states of the minimax algorithm
    //function to get the best move from a list of Moves
    private Move GetBestMove(List<Move> list)
    {

        /*
        print("All Possible moves are");
        foreach (Move n in list)
        {
            int[] resul = new int[4];

            resul[0] = n.GetOrigin().x;
            resul[1] = n.GetOrigin().y;
            resul[2] = n.GetDestination().x;
            resul[3] = n.GetDestination().y;



            Debug.Log(resul[0]);
            Debug.Log(resul[1]);
            Debug.Log("TO: ");
            Debug.Log(resul[2]);
            Debug.Log(resul[3]);
            Debug.Log(" ");
            Debug.Log("Move Score:");
            Debug.Log(n.GetScore());
            Debug.Log("----");


        }
        */




        Move result = new Move();
        result.AddScore(-999999);


        //loop through the list of final board states and return the move with the max score 
        foreach (Move move in list)
        {
            if ((move.GetScore() >= result.GetScore()))
            {
                result = move;
            }
        }
        return result;
    }






    private void ChildMove(Move move, bool isWhite, int depth)
    {



        //base case. add all the children moves into the global variable and return  
        if (depth <= 0)
        {
            finalList.Add(move);
            //print(move.GetScore());
            return;
        }




        FPiece[,] board = move.GetBoard();

        //Create a list of pieces of current color
        List<FPiece> pieces = new List<FPiece>();
        //get a list of origin pieces
        if (isWhite)
        {
            List<FPiece> tempWhites = GetWhitePieces(board);

            //check if the board is forced. Go through each piece to see if they are forced. 
            bool wasForced = false;
            foreach (FPiece x in tempWhites)
            {
                if (x.IsForceMovement(board, x.x, x.y))
                {
                    pieces.Add(x);
                    wasForced = true;
                }
            }

            //if the white pieces werent forced at all
            if (!wasForced)
            {
                pieces = tempWhites;
            }
        }
        else
        {
            List<FPiece> tempBlacks = GetBlackPieces(board);

            //check if the board is forced. Go through each piece to see if they are forced. 
            bool wasForced = false;
            foreach (FPiece x in tempBlacks)
            {
                if (x.IsForceMovement(board, x.x, x.y))
                {
                    pieces.Add(x);
                    wasForced = true;
                }
            }

            //if the white pieces werent forced at all
            if (!wasForced)
            {
                pieces = tempBlacks;
            }

        }



        List<Move> newList = new List<Move>(); //create new list to set the children of each of this current move


        //for each piece of the given color
        foreach (FPiece origin in pieces)
        {

            //Generate a list of all possible moves
            List<FPiece> destinations = GetPossibleMoves(origin, board);

            //Once you have a list of all possible moves. Carry out the virtual move on the board
            foreach (FPiece destination in destinations)
            {
                //copy the board
                FPiece[,] newBoard = CopyBoard(board);
                //make a new move and carry out a virual move on that board 
                Move newM = new Move(move.GetOrigin(), move.GetDestination(), newBoard);
                newM.AddScore(move.GetScore());
                //Make virtual move here and call the recursive function on the following: 
                Move newMove = MakeVirtualMove(origin, destination, newM);
                newList.Add(newMove);
            }
        }


        //call recursive function on each move in newList with depth-1, oppposite color.
        foreach (Move n in newList)
        {
            ChildMove(n, !isWhite, depth - 1);
        }

    }






    //Main function of the algorithm
    public int[] GetMove(Piece[,] mainBoard)
    {

        int[] result = new int[4];

        FPiece[,] board = CopyMainBoard(mainBoard);



        //Get a list of moveable black pieces
        List<FPiece> blackPieces = new List<FPiece>();
        List<FPiece> tempBlacks = GetBlackPieces(board);
        bool wasForced = false;
        foreach (FPiece x in tempBlacks)
        {
            if (x.IsForceMovement(board, x.x, x.y))
            {
                blackPieces.Add(x);
                wasForced = true;
            }
        }

        //if the black pieces werent forced at all
        if (!wasForced)
        {
            blackPieces = tempBlacks;
        }
        else
        {
            //check if it was forced and now only one white is left on board, then game won
            List<FPiece> whitePieces = GetWhitePieces(board);
            if (whitePieces.Count == 1)
            {

                List<FPiece> done = GetPossibleMoves(blackPieces[0], board);


                result[0] = blackPieces[0].x;
                result[1] = blackPieces[0].y;
                result[2] = done[0].x;
                result[3] = done[0].y;

                return result;

            }


        }





        List<Move> moves = new List<Move>();//list to store the list of moves

        //For each black piece on the board
        foreach (FPiece piece in blackPieces)
        {
            print("--");
            print("Possible black: ");
            print(piece.x);
            print(piece.y);
            print("--");
            List<FPiece> destinations = GetPossibleMoves(piece, board);

            //for each destination
            foreach (FPiece destination in destinations)
            {

                FPiece[,] newBoard = CopyBoard(board);
                //make a new move and carry out a virual move on that board 


                //generate a phantom piece for the origin 
                //FPiece origin = new FPiece(piece.x, piece.y);

                Move newMove = new Move(piece, destination, newBoard);
                Move resultantMove = MakeVirtualMove(piece, destination, newMove);
                //print(resultantMove.GetScore());
                moves.Add(resultantMove); //add the virtual move into the board
            }
        }

        //now call the recursive function on each of the child moves carried out 
        foreach (Move thismove in moves)
        {
            ChildMove(thismove, true, 1);
            //print(thismove.GetScore());

        }


        //do the resetting, and returning here

        Move finalMove = GetBestMove(finalList); //getting the best move among the list of moves
        finalList = null; //resetting the global variable 






        result[0] = finalMove.GetOrigin().x;
        result[1] = finalMove.GetOrigin().y;
        result[2] = finalMove.GetDestination().x;
        result[3] = finalMove.GetDestination().y;


        Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        Debug.Log("BestMove: ");
        //Debug.Log(treeCount);
        Debug.Log(result[0]);
        Debug.Log(result[1]);
        Debug.Log("TO: ");
        Debug.Log(result[2]);
        Debug.Log(result[3]);
        Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");




        return result;
    }
}
