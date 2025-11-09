using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Entities;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Repositories;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Services
{
    public class ClubService
    {
        private readonly ClubRepository _clubRepository;
        private readonly UserRepository _userRepository;
        private readonly NotificationRepository _notificationRepository;
        public ClubService(ClubRepository clubRepository, UserRepository userRepository, NotificationRepository notificationRepository)
        {
            _clubRepository = clubRepository;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
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

            } while (await _clubRepository.IfJoinCodeTaken(joinCode));

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
        public async Task Delete(Guid clubId)
        {
            try
            {
                await _clubRepository.Delete(clubId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        public async Task AddMember(Guid UserId, Guid ClubId)
        {
            var user = await _userRepository.GetById(UserId);
            var club = await _clubRepository.GetById(ClubId);
            _ = club ?? throw new Exception("Can't find a club using provided join code");
            _ = user ?? throw new Exception("Can't add user to a club");

            if (!await _clubRepository.ContainsUser(club, user))
            {
                await _clubRepository.AddUser(user, club);
                //Create notification to a user who sent a request
            }
            else
            {
                throw new Exception("User is already a member of the club");
            }
        }

        public async Task DeleteMember(Guid clubId, Guid userId)
        {
            await _clubRepository.DeleteUser(clubId, userId);
            //Create notification to user who was deleted from team
        }
    }
}
