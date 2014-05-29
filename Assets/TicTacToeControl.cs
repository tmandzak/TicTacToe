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
		bool widthSmaller;
		float smallSide;
		float button_height;
		float titleImage_height;
		float titleImage_width;
		float button_width;
		GUIStyle labelStyle;
	
		public void AdjustSizes ()
		{
				widthSmaller = Screen.width < Screen.height;
				smallSide = widthSmaller ? Screen.width : Screen.height;
	
				button_height = 75;
				titleImage_height = titleImage.height + button_height > Screen.height ? Screen.height - button_height - 5 : titleImage.height;
				titleImage_width = titleImage.width > Screen.width ? Screen.width - 5 : titleImage.width;
				button_width = titleImage_width;
		
				labelStyle = new GUIStyle (GUI.skin.GetStyle ("label"));
				labelStyle.fontSize *= (int)((Screen.width + Screen.height - smallSide * 2) / 100);
		}
	
		public void OnGUI ()
		{
				if (guiSkin != null)
						GUI.skin = guiSkin;
			
				AdjustSizes ();
				
				switch (gameState) {
				case GameState.Opening:
						DrawOpening ();
						break;
			
				case GameState.Multiplayer:
						DrawGameBoard ();
						break;
						
				case GameState.Singleplayer:
						DrawGameBoard ();
						break;		
			
				case GameState.GameOver:
						DrawGameOver ();
						break;
				}
		}
	
		public void DrawOpening ()
		{
				Rect groupRect = new Rect ((Screen.width / 2) - (titleImage_width / 2), (Screen.height / 2) - ((titleImage_height + button_height * 2 + 5) / 2), titleImage.width, titleImage.height + button_height * 2 + 5);
				GUI.BeginGroup (groupRect);
		
				Rect titleRect = new Rect (0, 0, titleImage_width, titleImage_height);
				GUI.DrawTexture (titleRect, titleImage, ScaleMode.ScaleToFit);
		
				Rect multiRect = new Rect (titleRect.x, titleRect.y + titleRect.height, button_width, button_height);
				Rect singleRect = new Rect (multiRect.x, multiRect.y + multiRect.height + 5, button_width, button_height);

				if (GUI.Button (multiRect, "Multy Game")) {
						NewGame ();
						gameState = GameState.Multiplayer;
				}
				
				if (GUI.Button (singleRect, "Single Game")) {
						NewGame ();
						gameState = GameState.Singleplayer;
				}
		
				GUI.EndGroup ();
		}

		public void DrawGameBoard ()
		{
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
				turnRect.height = widthSmaller ? Screen.height - Screen.width : Screen.height;
		
				string turnTitle = xTurn ? "X's Turn!" : "O's Turn!";
				GUI.Label (turnRect, turnTitle, labelStyle);
		
		}
	
		public void DrawGameOver ()
		{
				Rect groupRect = new Rect ((Screen.width - button_width) / 2, (Screen.height / 2) - (titleImage_height / 2 - button_height), button_width, button_height * 3);
				GUI.BeginGroup (groupRect);
		
				Rect winnerRect = new Rect (0, 0, button_width, button_height);
		
				string winnerTitle = (winner == SquareState.XControl) ? "X Wins!" : (winner == SquareState.OControl) ? "O Wins!" : "It's A Tie!";
				GUI.Label (winnerRect, winnerTitle, labelStyle);
		
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
		// Update is called once per frame
		void Update ()
		{
				if (gameState == GameState.Singleplayer) {
						int choice;
						if (xTurn) {
								do {
										choice = Random.Range (0, 8);
								} while(board[choice]!=SquareState.Clear);
								SetControl (choice);
						}
				}
		}
	
		void LateUpdate ()
		{
				if (gameState != GameState.Multiplayer && gameState != GameState.Singleplayer)
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
