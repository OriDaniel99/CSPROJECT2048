using System.Windows;
using _2048MatrixPermutator;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WPFTest2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;

        public MainWindow()
        {
            vm = new ViewModel();
            DataContext = vm;
            InitializeComponent();
        }

        public void Print()
        {
            vm.UpdateTiles();
        }

        //START BUTTON
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                ItemsControlThing.ItemsSource = vm.MainBoard.ObservableMat;
                vm.Init();
                vm.MainBoard.Generate();
                vm.MainBoard.UpdateTiles();
                Print();
            }
           
        }

        //Run algorithm
        private void AlgoStart_Click(object sender, RoutedEventArgs e)
        {
            Algorithm();
        }

        #region Manual Movement
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Right)
            {
                vm.MainBoard.RightMove();
                vm.MainBoard.UpdateTiles();
            }
            else if (e.Key == System.Windows.Input.Key.Left)
            {
                vm.MainBoard.LeftMove();
                vm.MainBoard.UpdateTiles();
            }
            else if (e.Key == System.Windows.Input.Key.Up)
            {
                vm.MainBoard.UpMove();
                vm.MainBoard.UpdateTiles();
            }
            else if (e.Key == System.Windows.Input.Key.Down)
            {
                vm.MainBoard.DownMove();
                vm.MainBoard.UpdateTiles();
            }
            else if (e.Key == System.Windows.Input.Key.K)
            {
                vm.MainBoard.UpdateTiles();
            }
        }
        #endregion

        #region Algorithm1
        //apply after each board change
        public void ChangeMainBoard(Board board)
        {
            vm.MainBoard = board;
            vm.MainBoard.UpdateTiles();
            ItemsControlThing.ItemsSource = board.ObservableMat;
        }
        //list of 1024 boards
        public List<Board> TempBoards = new List<Board>();
        #region Generate Move
        //Left
        public Board GenerateNewLeft(Board board)
        {
            Board temp = new Board(board);
            temp.Left();
            return temp;
        }
        //Right
        public Board GenerateNewRight(Board board)
        {
            Board temp = new Board(board);
            temp.Right();
            return temp;
        }
        //Up   
        public Board GenerateNewUp(Board board)
        {
            Board temp = new Board(board);
            temp.Up();
            return temp;
        }
        //Down 
        public Board GenerateNewDown(Board board)
        {
            Board temp = new Board(board);
            temp.Down();
            return temp;
        }
        #endregion
        public void GenerateRecursion(Board board, int currentDepth)
        {
            if(currentDepth == 5)
            {
                TempBoards.Add(board);
                return;
            }
            GenerateRecursion(GenerateNewLeft(board), currentDepth + 1);
            GenerateRecursion(GenerateNewUp(board), currentDepth + 1);
            GenerateRecursion(GenerateNewRight(board), currentDepth + 1);
            GenerateRecursion(GenerateNewDown(board), currentDepth + 1);
        }

        public Board GetBestBoard(Board board)
        {
            GenerateRecursion(board, 0);
            Board temp = TempBoards[0];
            for (int i = 0; i < TempBoards.Count; i++)
            {
                if (TempBoards[i].FinalScore > temp.FinalScore) temp = TempBoards[i];
            }
            TempBoards.Clear();
            return temp;
        }

        public void Algorithm()
        {
            Board board = vm.MainBoard;
            int runs = 0;
            while(/*vm.MainBoard.HighestNumber() != 2048 &&*/ runs < 1)
            {
                Board temp = new Board(vm.MainBoard);
                temp = GetBestBoard(temp);
                ChangeMainBoard(temp);
                runs++;
            }
        }
        #endregion
        /*#region Algorithm2
        public Stack<Board> ListOfBoards { get; set; } = new Stack<Board>();
        public Board finalBoard { get; set; } = new Board();
        public bool GameWon(Board board)
        {
            if (board.HighestNumber() == 2048) return true;
            return false;
        }

        public void Algorithm2(Board board, int depth)
        {
            if (GameWon(board) || depth == 100)
            {
                vm.MainBoard = board;
            }
            //LEFT MOVE
            if (board.LeftMoveAvailable())
            {
                board.PrevBoard = board;
                board.LeftMove();
                Algorithm2(board, depth + 1);
            }
            //UP MOVE
            if (board.UpMoveAvailable())
            {
                board.PrevBoard = board;
                board.UpMove();
                Algorithm2(board, depth + 1);
            }
            //RIGHT MOVE
            if (board.RightMoveAvailable())
            {
                board.PrevBoard = board;
                board.RightMove();
                Algorithm2(board, depth + 1);
            }
            //DOWN MOVE 
            if (board.DownMoveAvailable())
            {
                board.PrevBoard = board;
                board.DownMove();
                Algorithm2(board, depth + 1);
            }
        }

        public void IntoList(Board board)
        {
            while (board.PrevBoard != null)
            {
                ListOfBoards.Push(board);
                board = board.PrevBoard;
            }
        }
        #endregion*/

       
    }
}
