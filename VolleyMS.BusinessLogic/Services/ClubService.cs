using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Repositories;

namespace VolleyMS.BusinessLogic.Services
{
    public class ClubService
    {
        private readonly ClubRepository _clubRepository;
        private readonly UserRepository _userRepository;
        public ClubService(ClubRepository clubRepository, UserRepository userRepository) 
        {
            _clubRepository = clubRepository;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateJoinCode()
        {
            string joinCode = string.Empty;
            const string salt = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();

            do
            {
                joinCode = new string(Enumerable.Range(0, 9)
                                        .Select(_ => salt[random.Next(salt.Length)])
                                        .ToArray());

            } while(await _clubRepository.IfJoinCodeTaken(joinCode));

            return joinCode;
        }

        public async Task Create(Club club)
        {
            string joinCode = await GenerateJoinCode();
            var clubTuple = Club.Create(club.Id, club.Name, joinCode, club.Description, club.AvatarURL, club.BackGroundURL);
            
            if (clubTuple.error == string.Empty)
            {
                await _clubRepository.Create(clubTuple.club);
            }
            else
            {
                throw new Exception(clubTuple.error);
            }
        }

        public async Task AddUser(Guid Id, string joinCode)
        {
            var user = await _userRepository.GetById(Id);
            var club = await _clubRepository.GetClubByCode(joinCode);
            _ = club ?? throw new Exception("Can't find a club using provided join code");
            _ = user ?? throw new Exception("Can't add user to a club"); 

            if (!await _clubRepository.ContainsUser(club, user))
            {
                await _clubRepository.AddUser(user, joinCode); // 100% not null, i check its value before.
            }
            else
            {
                throw new Exception("User is already a member of the club");
            }
        }
    }
}
