using Microsoft.AspNetCore.Http;
using Movies.Client.Extensions;
using Movies.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class MovieService : IMovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var movieList = await Get<List<Movie>>("/movies");

            return movieList;
        }

        public async Task<Movie> GetMovie(int id)
        {
            var movie = await Get<Movie>(string.Format("/movies/{0}", id));

            return movie;
        }

        public async Task<bool> CreateMovie(Movie movie)
        {
            await Execute<Movie>(HttpMethod.Post, "/movies", movie);

            return true;
        }

        public async Task<bool> UpdateMovie(int id, Movie movie)
        {
            await Execute<Movie>(HttpMethod.Put, string.Format("/movies/{0}", id), movie);

            return true;
        }

        public async Task<bool> DeleteMovie(int id)
        {
            await Execute<Movie>(HttpMethod.Delete, string.Format("/movies/{0}", id), default);

            return true;
        }

        private async Task<T> Get<T>(string uri)
        {
            return await Execute<T>(HttpMethod.Get, uri, default);
        }

        private async Task<T> Execute<T>(HttpMethod method, string uri, T data)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(method, uri);

            if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                request.SerializeData<T>(data);
            }

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.ReadContentAs<T>();
            }
            else
            {
                return default;
            }
        }
    }
}
