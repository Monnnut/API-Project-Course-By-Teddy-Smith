using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRespository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (commentModel == null)
            {
                return null;
            }

            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(a => a.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null)
            {
                return null;
            }

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        }
    }

}


//     AddAsync: Asynchronously adds a new entity (e.g., a record) to the database.
// SaveChangesAsync: Asynchronously saves all changes made in the context to the database.
// DeleteAsync: Asynchronously deletes an entity from the database.
// FirstOrDefaultAsync: Asynchronously returns the first entity that matches a condition or null if none is found.
// GetAllAsync: Asynchronously retrieves all entities from the database (typically a custom method).
// ToListAsync: Asynchronously converts the result of a query to a list.
// getbyidAsync: Asynchronously retrieves an entity by its ID (typically a custom method).
// AnyAsync: Asynchronously checks if any entities in the database match a given condition.
// UpdateAsync: Asynchronously updates an existing entity in the database.
// }