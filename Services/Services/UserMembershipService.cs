﻿using Core.Interfaces;
using Core.Response;
using MSSQLServer.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserMembershipService : IUserMembershipService
    {
        private readonly IUserMembershipRepository _userMembershipRepository;

        public UserMembershipService(IUserMembershipRepository userMembershipRepository)
        {
            _userMembershipRepository = userMembershipRepository;
        }

        public async Task<UserMembership> AddAsync(UserMembership user)
        {
            return await _userMembershipRepository.AddAsync(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _userMembershipRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserMembership>> GetAllAsync()
        {
            return await _userMembershipRepository.GetAllAsync();
        }

        public async Task<UserMembership?> GetByIdAsync(int id)
        {
            return await _userMembershipRepository.GetByIdAsync(id);
        }

        public Task<UserMembership> GetCurrentMembershipAsync(int userId)
        {
            return _userMembershipRepository.GetCurrentMembershipAsync(userId);
        }

        public async Task<UserMembershipResponse> GetListAsync(string? searchTypeName, DateOnly? assignment, int pageIndex, int pageSize)
        {
            return await _userMembershipRepository.GetListAsync(searchTypeName, assignment, pageIndex, pageSize);
        }

        public Task<List<UserMembership>> GetMembershipsByUserIdAsync(int userId)
        {
            return _userMembershipRepository.GetMembershipsByUserIdAsync(userId);
        }

        public async Task<UserMembership> UpdateAsync(UserMembership user)
        {
            return await _userMembershipRepository.UpdateAsync(user);
        }
    }
}
