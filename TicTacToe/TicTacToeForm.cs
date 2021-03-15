using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TicTacToeForm : Form
    {
        public int turnCounter = 0; // keeps track of turns played for player and tie purposes.
        public int[,] container = new int[3,3]; // our array that will store played pieces
        List<int> availableSpaces = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }; // list of all spaces so that the computer can chose from them, removed as moves are played
        
        /// <summary>
        /// Initialize form. Nothing special goes on here.
        /// </summary>
        public TicTacToeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This event occurs when the user clicks any of the picture boxes. Detects which picture box was clicked, places an X or O in it,
        /// and calls the ArrayContainer and WinCheck methods to store the placement and check for victory. Also shows a message box for victory
        /// if it occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox selectedSpace = sender as PictureBox; // this allows us to pull the specific one they clicked
            int tag = int.Parse(selectedSpace.Tag.ToString());
            turnCounter++; // put this here because if it was at the end then I had to put a bunch of return statements or it would increment once for the next game and O would start
            selectedSpace.Image = TicTacToe.Properties.Resources.x;
            selectedSpace.Enabled = false;

            ArrayContainer(tag);
            if (WinCheck() == true) // checks against the wincondition
            {
                MessageBox.Show("X wins!");
                Clear();
                return;
            }
            else if (turnCounter == 9) // x always plays last so there's no reason to repeat this below
            {
                MessageBox.Show("It's a tie!");
                Clear();
                return;
            }

            turnCounter++;

            if (turnCounter == 2) // first turn
            {
                if (int.Parse(selectedSpace.Tag.ToString()) == 4) // AI will always play in a corner if you played in the middle
                {
                    Random rand = new Random();
                    int temp = rand.Next(1, 4);
                    int placement;
                    switch(temp) // random 1-4 just to put in a random corner for some variety
                    {
                        case 1:
                            placement = 0;
                            break;
                        case 2:
                            placement = 2;
                            break;
                        case 3:
                            placement = 6;
                            break;
                        case 4:
                            placement = 8;
                            break;
                        default:
                            placement = 0;
                            break;
                    }

                    ArrayContainer(placement);
                    foreach (PictureBox item in this.Controls) // I don't know how to pull the item via tag without running through all of them
                    {
                        if (int.Parse(item.Tag.ToString()) == placement)
                        {
                            item.Image = TicTacToe.Properties.Resources.o;
                            item.Enabled = false;
                        }
                    }
                }
                else // will play in the middle if you didn't
                {
                    ArrayContainer(4);
                    foreach (PictureBox item in this.Controls) // I don't know how to pull the item via tag without running through all of them
                    {
                        if (int.Parse(item.Tag.ToString()) == 4)
                        {
                            item.Image = TicTacToe.Properties.Resources.o;
                            item.Enabled = false;
                        }
                    }
                }
            }


            if (turnCounter != 2) // any turn after 2
            {
                ComputerTurn();
                if (WinCheck() == true)
                {
                    MessageBox.Show("Computer Wins");
                    Clear();
                }
            }
        }

        /// <summary>
        /// Using the tag that we pulled, places the played character into an array holding positions. This allows us to use them to check positions later. 
        /// </summary>
        /// <param name="tag">The tag pulled from the picture box, saying its position</param>
        public void ArrayContainer(int tag)
        {
            int player;
            if(turnCounter%2 != 0) // used to say whether we are placing a 1 or a 2 in each row. This is important, because if we use the same number then
                                   // it will 'win' whenever there are 3 in a row regardless of if they match
            {
                player = 1;
            }
            else
            {
                player = 2;
            }

            switch (tag) // places a number into our 2D array based on where they played
            {
                case 0:
                    container[0, 0] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(0));
                    break;
                case 1:
                    container[0, 1] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(1));
                    break;
                case 2:
                    container[0, 2] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(2));
                    break;
                case 3:
                    container[1, 0] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(3));
                    break;
                case 4:
                    container[1, 1] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(4));
                    break;
                case 5:
                    container[1, 2] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(5));
                    break;
                case 6:
                    container[2, 0] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(6));
                    break;
                case 7:
                    container[2, 1] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(7));
                    break;
                case 8:
                    container[2, 2] = player;
                    availableSpaces.RemoveAt(availableSpaces.IndexOf(8));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Checks against every win condition individually, by testing if each row is the same, and not zero. Since X and Os are different numbers, this works.
        /// </summary>
        /// <returns>Returns true if win condition met, false if not.</returns>
        public bool WinCheck()
        {
            bool win = false;
            if(container[0,0]==container[0,1] && container[0,0]==container[0,2] && container[0,0] != 0)
            {
                win = true;
            }
            else if (container[1, 0] == container[1, 1] && container[1, 0] == container[1, 2] && container[1, 0] != 0)
            {
                win = true;
            }
            else if (container[2, 0] == container[2, 1] && container[2, 0] == container[2, 2] && container[2, 0] != 0)
            {
                win = true;
            }
            else if (container[0, 0] == container[1, 0] && container[0, 0] == container[2, 0] && container[0, 0] != 0)
            {
                win = true;
            }
            else if (container[0, 1] == container[1, 1] && container[0, 1] == container[2, 1] && container[0, 1] != 0)
            {
                win = true;
            }
            else if (container[0, 2] == container[1, 2] && container[0, 2] == container[2, 2] && container[0, 2] != 0)
            {
                win = true;
            }
            else if (container[0, 0] == container[1, 1] && container[0, 0] == container[2, 2] && container[0, 0] != 0)
            {
                win = true;
            }
            else if (container[0, 2] == container[1, 1] && container[0, 2] == container[2, 0] && container[0, 2] != 0)
            {
                win = true;
            }
            return win;
        }

        /// <summary>
        /// This will clear all of the played Xs and Os, as well as resetting the turn counter and resetting the array. Effectively starts the game over.
        /// </summary>
        public void Clear()
        {
            foreach(PictureBox item in this.Controls) // resets the picture in each picturebox
            {
                item.Image = null;
                item.Enabled = true;
            }
            container = new int[3, 3]; // creates a new array for our 'container' variable to point at. The old array gets cleaned up once unassociated.
                                       // I could have iterated through two for loops and set them all to 0, but this was easier and I'm lazy :)
            turnCounter = 0;
            availableSpaces = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }; // creates a new list, same as array. This one actually is necessary, as we've deleted a bunch from the old one.
        }

        /// <summary>
        /// Plays the computer move. Checks against the game state and plays accordingly, or plays randomly.
        /// </summary>
        public void ComputerTurn()
        {
            int computerMove;
            int temp = ComputerCheck();
            
            if (temp != 0) // if the check found a state listed, it will have a specific move
            {
                computerMove = temp - 1; // we used base 1 so we have to reduce 1 to be base 0
            }
            else // if it didn't, it'll chose a random number from available
            {
                Random randomNum = new Random();
                int rand = randomNum.Next(0, availableSpaces.Count);
                computerMove = availableSpaces[rand];
            }
            Console.WriteLine(computerMove);

            ArrayContainer(computerMove);

            foreach (PictureBox item in this.Controls)
            {
                if (int.Parse(item.Tag.ToString()) == computerMove)
                {
                    item.Image = TicTacToe.Properties.Resources.o;
                    item.Enabled = false;
                }
            }
            
        }

        /// <summary>
        /// Checks the game state and returns computer move depending on state
        /// </summary>
        /// <returns>Returns square to play in base 1.</returns>
        public int ComputerCheck()
        {
            int returnNum = 0;
            // test horizontal wins
            if (container[0, 0] == 2 && container[0, 1] == 2 && container[0, 2] == 0) { returnNum = 3; }
            else if (container[0, 1] == 2 && container[0, 2] == 2 && container[0, 0] == 0) { returnNum = 1; }
            else if (container[1, 0] == 2 && container[1, 1] == 2 && container[1, 2] == 0) { returnNum = 6; }
            else if (container[1, 1] == 2 && container[1, 2] == 2 && container[1, 0] == 0) { returnNum = 4; }
            else if (container[2, 0] == 2 && container[2, 1] == 2 && container[2, 2] == 0) { returnNum = 9; }
            else if (container[2, 1] == 2 && container[2, 2] == 2 && container[2, 0] == 0) { returnNum = 7; }
            // vertical
            else if (container[0, 0] == 2 && container[1, 0] == 2 && container[2, 0] == 0) { returnNum = 7; }
            else if (container[1, 0] == 2 && container[2, 0] == 2 && container[0, 0] == 0) { returnNum = 1; }
            else if (container[0, 1] == 2 && container[1, 1] == 2 && container[2, 1] == 0) { returnNum = 8; }
            else if (container[2, 1] == 2 && container[1, 1] == 2 && container[0, 1] == 0) { returnNum = 2; }
            else if (container[0, 2] == 2 && container[1, 2] == 2 && container[2, 2] == 0) { returnNum = 9; }
            else if (container[2, 2] == 2 && container[1, 2] == 2 && container[0, 2] == 0) { returnNum = 3; }
            // crisscross
            else if (container[0, 0] == 2 && container[1, 1] == 2 && container[2, 2] == 0) { returnNum = 9; }
            else if (container[2, 2] == 2 && container[1, 1] == 2 && container[0, 0] == 0) { returnNum = 1; }
            else if (container[2, 0] == 2 && container[1, 1] == 2 && container[0, 2] == 0) { returnNum = 3; }
            else if (container[0, 2] == 2 && container[1, 1] == 2 && container[2, 0] == 0) { returnNum = 7; }
            // horizontal missing center
            else if (container[0, 0] == 2 && container[0, 2] == 2 && container[0, 1] == 0) { returnNum = 2; }
            else if (container[1, 0] == 2 && container[1, 2] == 2 && container[1, 1] == 0) { returnNum = 5; }
            else if (container[2, 0] == 2 && container[2, 2] == 2 && container[2, 1] == 0) { returnNum = 8; }
            // vertical missing center
            else if (container[0, 0] == 2 && container[2, 0] == 2 && container[1, 0] == 0) { returnNum = 4; }
            else if (container[0, 1] == 2 && container[2, 1] == 2 && container[1, 1] == 0) { returnNum = 5; }
            else if (container[0, 2] == 2 && container[2, 2] == 2 && container[1, 2] == 0) { returnNum = 6; }
            // criss cross missing center
            else if (container[0, 0] == 2 && container[2, 2] == 2 && container[1, 1] == 0) { returnNum = 5; }
            else if (container[0, 2] == 2 && container[2, 0] == 2 && container[1, 1] == 0) { returnNum = 5; }

            // repeat to block enemy victory
            else if (container[0, 0] == 1 && container[0, 1] == 1 && container[0, 2] == 0) { returnNum = 3; }
            else if (container[0, 1] == 1 && container[0, 2] == 1 && container[0, 0] == 0) { returnNum = 1; }
            else if (container[1, 0] == 1 && container[1, 1] == 1 && container[1, 2] == 0) { returnNum = 6; }
            else if (container[1, 1] == 1 && container[1, 2] == 1 && container[1, 0] == 0) { returnNum = 4; }
            else if (container[2, 0] == 1 && container[2, 1] == 1 && container[2, 2] == 0) { returnNum = 9; }
            else if (container[2, 1] == 1 && container[2, 2] == 1 && container[2, 0] == 0) { returnNum = 7; }

            else if (container[0, 0] == 1 && container[1, 0] == 1 && container[2, 0] == 0) { returnNum = 7; }
            else if (container[1, 0] == 1 && container[2, 0] == 1 && container[0, 0] == 0) { returnNum = 1; }
            else if (container[0, 1] == 1 && container[1, 1] == 1 && container[2, 1] == 0) { returnNum = 8; }
            else if (container[2, 1] == 1 && container[1, 1] == 1 && container[0, 1] == 0) { returnNum = 2; }
            else if (container[0, 2] == 1 && container[1, 2] == 1 && container[2, 2] == 0) { returnNum = 9; }
            else if (container[2, 2] == 1 && container[1, 2] == 1 && container[0, 2] == 0) { returnNum = 3; }

            else if (container[0, 0] == 1 && container[1, 1] == 1 && container[2, 2] == 0) { returnNum = 9; }
            else if (container[2, 2] == 1 && container[1, 1] == 1 && container[0, 0] == 0) { returnNum = 1; }
            else if (container[2, 0] == 1 && container[1, 1] == 1 && container[0, 2] == 0) { returnNum = 3; }
            else if (container[0, 2] == 1 && container[1, 1] == 1 && container[2, 0] == 0) { returnNum = 7; }

            else if (container[0, 0] == 1 && container[0, 2] == 1 && container[0, 1] == 0) { returnNum = 2; }
            else if (container[1, 0] == 1 && container[1, 2] == 1 && container[1, 1] == 0) { returnNum = 5; }
            else if (container[2, 0] == 1 && container[2, 2] == 1 && container[2, 1] == 0) { returnNum = 8; }

            else if (container[0, 0] == 1 && container[2, 0] == 1 && container[1, 0] == 0) { returnNum = 4; }
            else if (container[0, 1] == 1 && container[2, 1] == 1 && container[1, 1] == 0) { returnNum = 5; }
            else if (container[0, 2] == 1 && container[2, 2] == 1 && container[1, 2] == 0) { returnNum = 6; }

            else if (container[0, 0] == 1 && container[2, 2] == 1 && container[1, 1] == 0) { returnNum = 5; }
            else if (container[0, 2] == 1 && container[2, 0] == 1 && container[1, 1] == 0) { returnNum = 5; }

            // fork check
            else if (container[0, 0] == 1 && container[1, 1] == 1 && container[2, 2] == 2 && container[2, 0] == 0 && container[0, 2] == 0) { returnNum = 3; }
            else if (container[2, 2] == 1 && container[1, 1] == 1 && container[0, 0] == 2 && container[2, 0] == 0 && container[0, 2] == 0) { returnNum = 7; }
            else if (container[2, 0] == 1 && container[1, 1] == 1 && container[0, 2] == 2 && container[0, 0] == 0 && container[2, 2] == 0) { returnNum = 1; }
            else if (container[0, 2] == 1 && container[1, 1] == 1 && container[2, 0] == 2 && container[0, 0] == 0 && container[2, 2] == 0) { returnNum = 9; }

            else if (container[0, 0] == 1 && container[2, 2] == 1 && container[1, 1] == 2 && container[0, 2] == 0 && container[2, 0] == 0) { returnNum = 2; }
            else if (container[2, 0] == 1 && container[0, 2] == 1 && container[1, 1] == 2 && container[0, 0] == 0 && container[2, 2] == 0) { returnNum = 8; }
            // fork attempts
            else if (container[0, 0] == 2 && container[1, 1] == 2 && container[2, 2] == 1 && container[2, 0] == 0 && container[0, 2] == 0) { returnNum = 3; }
            else if (container[2, 2] == 2 && container[1, 1] == 2 && container[0, 0] == 1 && container[2, 0] == 0 && container[0, 2] == 0) { returnNum = 7; }
            else if (container[2, 0] == 2 && container[1, 1] == 2 && container[0, 2] == 1 && container[0, 0] == 0 && container[2, 2] == 0) { returnNum = 1; }
            else if (container[0, 2] == 2 && container[1, 1] == 2 && container[2, 0] == 1 && container[0, 0] == 0 && container[2, 2] == 0) { returnNum = 9; }
            return returnNum;
        }
    }
}