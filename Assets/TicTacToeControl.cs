using UnityEngine;
using System.Collections;

public class TicTacToeControl : MonoBehaviour
{
		public SquareState[] board = new SquareState[9];
		public bool xTurn = true;
		public GameState gameState = GameState.Opening;
		public SquareState winner = SquareState.Clear;
		public GUISkin guiSkin;
		public Texture2D titleImage;

		public void OnGUI ()
		{
				if (guiSkin != null)
						GUI.skin = guiSkin;
						
				switch (gameState) {
				case GameState.Opening:
						DrawOpening ();
						break;
		
				case GameState.Multiplayer:
						DrawGameBoard ();
						break;

				case GameState.GameOver:
						DrawGameOver ();
						break;
				}
		}

		public void DrawOpening ()
		{
				Rect groupRect = new Rect ((Screen.width / 2) - (titleImage.width / 2), (Screen.height / 2) - ((titleImage.height + 75) / 2), titleImage.width, titleImage.height + 75);
				GUI.BeginGroup (groupRect);
		
				Rect titleRect = new Rect (0, 0, titleImage.width, titleImage.height);
				//GUI.Label (titleRect, "Tic-Tac-Toe");
				GUI.DrawTexture (titleRect, titleImage);

				Rect multiRect = new Rect (titleRect.x, titleRect.y + titleRect.height, titleRect.width, 75);

				if (GUI.Button (multiRect, "New Game")) {
						NewGame ();
						gameState = GameState.Multiplayer;
				}

				GUI.EndGroup ();
		}

		public void DrawGameBoard ()
		{
				bool widthSmaller = Screen.width < Screen.height;
				float smallSide = widthSmaller ? Screen.width : Screen.height;
		
				//float width = 75;
				//float height = 75;
				float width = smallSide / 3;
				float height = width;
				
				for (int y=0; y<3; y++) {
						for (int x=0; x<3; x++) {
								int boardIndex = (y * 3) + x;
								Rect square = new Rect (x * width, y * height, width, height);
								string owner = board [boardIndex] == SquareState.XControl ? "X" : (board [boardIndex] == SquareState.OControl ? "O" : ""); 

								if (board [boardIndex] == SquareState.Clear) {
										if (GUI.Button (square, owner)) 
												SetControl (boardIndex); 
								} else
										GUI.Label (square, owner, owner + "Square");
						}
				}

				Rect turnRect = new Rect (300, 0, 100, 100);
				
				turnRect.x = widthSmaller ? 0 : smallSide;
				turnRect.y = widthSmaller ? smallSide : 0;
				turnRect.width = widthSmaller ? Screen.width : Screen.width - Screen.height;
				turnRect.height = widthSmaller ? Screen.height-Screen.width : Screen.height;
				
				
				string turnTitle = xTurn ? "X's Turn!" : "O's Turn!";
				GUI.Label (turnRect, turnTitle);

		}

		public void DrawGameOver ()
		{
				Rect groupRect = new Rect ((Screen.width / 2) - 150, (Screen.height / 2) - (titleImage.height / 2 - 75), 300, 150);
				GUI.BeginGroup (groupRect);
		
				//Rect winnerRect = new Rect (0, 0, 300, 75);
		Rect winnerRect = new Rect (0, 0, groupRect.width, groupRect.height/2);
				
				string winnerTitle = winner == SquareState.XControl ? "X Wins!" : winner == SquareState.OControl ? "O Wins!" : "It's A Tie!";
				GUI.Label (winnerRect, winnerTitle);

				winnerRect.y += winnerRect.height;
				if (GUI.Button (winnerRect, "Main Menu"))
						gameState = GameState.Opening;
						
				GUI.EndGroup ();

		}

		public void SetControl (int boardIndex)
		{
				if (boardIndex < 0 || boardIndex >= board.Length)
						return;
				board [boardIndex] = xTurn ? SquareState.XControl : SquareState.OControl;
				xTurn = !xTurn;
		}

		public void NewGame ()
		{
				xTurn = true;
				board = new SquareState[9];
		}

		public void SetWinner (SquareState toWin)
		{
				winner = toWin;
				gameState = GameState.GameOver;
		}


//		// Use this for initialization
//		void Start ()
//		{
//	
//		}
//	
//		// Update is called once per frame
//		void Update ()
//		{
//	
//		}

		void LateUpdate ()
		{
				if (gameState != GameState.Multiplayer)
						return;
				for (int i=0; i<3; i++) {
						if (board [i] != SquareState.Clear && board [i] == board [i + 3] && board [i] == board [i + 6]) {
								SetWinner (board [i]);
								return;
						} else if (board [i * 3] != SquareState.Clear && board [i * 3] == board [i * 3 + 1] && board [i * 3] == board [i * 3 + 2]) {
								SetWinner (board [i * 3]);
								return;
						}
				}

				if (board [0] != SquareState.Clear && board [0] == board [4] && board [0] == board [8]) {
						SetWinner (board [0]);
						return;

				} else if (board [2] != SquareState.Clear && board [2] == board [4] && board [2] == board [6]) {
						SetWinner (board [2]);
						return;
				}

				for (int i=0; i<board.Length; i++) {
						if (board [i] == SquareState.Clear)
								return;

				}
				SetWinner (SquareState.Clear);
		}
}
