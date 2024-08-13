using BillingApp.Core.Common;
using BillingApp.Core.Entities;
using BillingApp.Core.Exceptions;
using BillingApp.Core.Interfaces;
using BillingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Infrastructure.Repositories;

 public class BillRepository : IBillRepository
    {
        private readonly AppDbContext _context;

        public BillRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultType<IEnumerable<Bill>>> GetAllAsync()
        {
            var bills = await _context.Bills.ToListAsync();
            return ResultType<IEnumerable<Bill>>.Success(bills);
        }

        public async Task<ResultType<Bill>> GetByIdAsync(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                throw new CustomException($"Bill with id {id} not found", 404);
            }
            return ResultType<Bill>.Success(bill);
        }

        public async Task<ResultType<int>> AddAsync(Bill bill)
        {
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();
            return ResultType<int>.Success(bill.Id);
        }

        public async Task<ResultType<Bill>> UpdateAsync(Bill bill)
        {
            _context.Entry(bill).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return ResultType<Bill>.Success(bill);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BillExists(bill.Id))
                {
                    throw new CustomException($"Bill with id {bill.Id} not found", 404);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<ResultType<bool>> DeleteAsync(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                throw new CustomException($"Bill with id {id} not found", 404);
            }

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return ResultType<bool>.Success(true);
        }

        private async Task<bool> BillExists(int id)
        {
            return await _context.Bills.AnyAsync(e => e.Id == id);
        }
    }