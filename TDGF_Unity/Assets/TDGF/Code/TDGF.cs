using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHTTP;

namespace MADD
{
    public class TDGF : Singleton<TDGF>
    {
        #region member variables

        [ReadOnly]
        public List<Game> _myGames;
        [ReadOnly]
        public Game _game;
        public string URL = "http://localhost:1337/api";
        public string _email;
        public string _password;

        [SerializeField]
        private string _token;
        private bool _loading = false;

        #endregion

        #region login and register

        public class LoginDetails
        {
            public LoginDetails(string mail, string passwd)
            {
                email = mail;
                identifier = mail;
                password = passwd;
            }
            public string email, identifier, password, token, jwt;
        }

        public void Register()
        {
            if (!_loading)
                StartCoroutine(RegisterCO());
        }

        public void Login()
        {
            if (!_loading)
                StartCoroutine(LoginCO());
        }

        public IEnumerator RegisterCO()
        {
            _loading = true;
            LoginDetails data = new LoginDetails(_email, _password);

            Request request = new Request(URL + "/auth/local/register")
                .Post(RequestBody.From(data));

            Client http = new Client();
            yield return http.Send(request);

            _loading = false;
            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                _token = resp.To<LoginDetails>().jwt;
            }
            else
            {
                Debug.LogWarning("error: " + http.Error());
            }
        }

        public IEnumerator LoginCO()
        {
            _loading = true;
            LoginDetails data = new LoginDetails(_email, _password);

            Request request = new Request(URL + "/auth/local")
                .Post(RequestBody.From(data));

            Client http = new Client();
            yield return http.Send(request);

            _loading = false;
            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                _token = resp.To<LoginDetails>().jwt;
            }
            else
            {
                Debug.LogWarning("error: " + http.Error());
            }
        }

        #endregion

        #region games management

        public void GetAllMyGames()
        {
            if (!_loading)
                StartCoroutine(GetAllMyGamesCO());
        }

        public void CreateGame(int locationsToGenerate)
        {
            if (!_loading)
                StartCoroutine(CreateGameCO(locationsToGenerate));
        }

        public IEnumerator GetAllMyGamesCO()
        {
            if (_token.Length == 0)
            {
                Debug.LogWarning("You need to login in order to fetch your games");
                yield break;
            }

            _loading = true;
            Request request = new Request(URL + "/games")
                .AddHeader("Authorization", "Bearer " + _token)
                .Get();

            Client http = new Client();
            yield return http.Send(request);

            _loading = false;
            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                print("Got games");
                _myGames = resp.ToArray<Game>();
            }
            else
            {
                Debug.LogWarning("error: " + http.Error());
            }
        }

        public IEnumerator CreateGameCO(int locationsToGenerate)
        {
            if (_token.Length == 0)
            {
                Debug.LogWarning("You need to login in order to create a game");
                yield break;
            }

            Game game = new Game();
            game.locationsAmount = locationsToGenerate;

            _loading = true;
            Request request = new Request(URL + "/games")
                .AddHeader("Authorization", "Bearer " + _token)
                .Post(RequestBody.From(game));

            Client http = new Client();
            yield return http.Send(request);

            _loading = false;
            if (http.IsSuccessful())
            {
                Response resp = http.Response();
                _game = resp.To<Game>();
            }
            else
            {
                Debug.LogWarning("error: " + http.Error());
            }
        }

        #endregion
    }
}