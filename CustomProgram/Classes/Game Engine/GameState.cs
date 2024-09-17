namespace SpaceDefenders
{
    // Responsible for controling boolean logic for game state
    public class GameState
    {
        private bool _gameStart;
        private bool _gamePaused;
        private bool _gameOver;
        private bool _gameMenu;
        private bool _gameTestMode;
        public GameState()
        {
            _gameMenu = true;
            _gameStart = false;
            _gamePaused = false;
            _gameOver = false;
            _gameTestMode = false;
        }

        public bool GameTestMode
        {
            get
            {
                return _gameTestMode;
            }

        }
        public bool GameStart
        {
            get
            {
                return _gameStart;
            }

        }

        public bool GameOver
        {
            get
            {
                return _gameOver;
            }
        }

        public bool GameMenu
        {
            get
            {
                return _gameMenu;
            }

        }

        public bool GamePaused
        {
            get
            {
                return _gamePaused;
            }

        }

        public void ReturnToMenu()
        {
            _gameStart = false;
            _gameOver = false;
            _gameMenu = true;
            _gamePaused = false;
            _gameTestMode = false;
        }

        public void StartGame()
        {
            _gameStart = true;
            _gameOver = false;
            _gameMenu = false;
            _gamePaused = false;
        }

        public void TogglePaused()
        {
            _gamePaused = !_gamePaused;
        }

        public void StartTestEnvironment()
        {
            _gameStart = true;
            _gameTestMode = true;
            _gamePaused = false;
            _gameMenu = false;
        }

        public void StartGameOver()
        {
            _gameStart = false;
            _gamePaused = false;
            _gameOver = true;
            _gameMenu = false;
            _gameTestMode= false;
        }
    }
}
