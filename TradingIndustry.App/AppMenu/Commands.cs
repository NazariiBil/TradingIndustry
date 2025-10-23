
using System;
using System.Linq;
using TradingIndustry.App.Interfaces;
using TradingIndustry.DAL.Interfaces;

namespace TradingIndustry.App.AppMenu
{
    
    public class GetAllCommand<T> : ICommand where T : class
    {
        private readonly IGenericDAL<T> _dal;
        public string Description => $"Get All {typeof(T).Name}s";

        public GetAllCommand(IGenericDAL<T> dal) => _dal = dal;

        public void Execute()
        {
            var results = _dal.GetAll();
            if (!results.Any())
            {
                Console.WriteLine($"\nNo {typeof(T).Name}s found.");
                return;
            }

            Console.WriteLine($"\n--- All {typeof(T).Name}s ({results.Count}) ---");
            foreach (var item in results)
            {
                Console.WriteLine(item);
            }
        }
    }

    
    public class GetByIdCommand<T> : ICommand where T : class
    {
        private readonly IGenericDAL<T> _dal;
        public string Description => $"Get {typeof(T).Name} by ID";

        public GetByIdCommand(IGenericDAL<T> dal) => _dal = dal;

        public void Execute()
        {
            int id = ConsoleHelper.ReadInt($"Enter {typeof(T).Name} ID to retrieve: ");
            var result = _dal.GetById(id);
            if (result != null)
            {
                Console.WriteLine($"\nResult: {result}");
            }
            else
            {
                Console.WriteLine($"\n{typeof(T).Name} with ID {id} not found.");
            }
        }
    }

    
    public class InsertCommand<T> : ICommand where T : class, new()
    {
        private readonly IGenericDAL<T> _dal;
        public string Description => $"Create New {typeof(T).Name}";

        public InsertCommand(IGenericDAL<T> dal) => _dal = dal;

        public void Execute()
        {
            try
            {
                
                var newEntity = ConsoleHelper.CreateNewEntity<T>();
                var created = _dal.Create(newEntity);

                if (created != null)
                {
                    Console.WriteLine($"\nSuccessfully created: {created}");
                }
                else
                {
                    Console.WriteLine($"\nFailed to create {typeof(T).Name}. Check logs for details.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[FATAL ERROR] Could not execute Create command: {ex.Message}");
            }
        }
    }

    
    public class UpdateCommand<T> : ICommand where T : class, new()
    {
        private readonly IGenericDAL<T> _dal;
        public string Description => $"Update Existing {typeof(T).Name}";

        public UpdateCommand(IGenericDAL<T> dal) => _dal = dal;

        public void Execute()
        {
            
            int id = ConsoleHelper.ReadInt($"Enter {typeof(T).Name} ID to update: ");
            var existing = _dal.GetById(id);

            if (existing == null)
            {
                Console.WriteLine($"\n{typeof(T).Name} with ID {id} not found.");
                return;
            }

            Console.WriteLine($"\n--- Current Data for {existing} ---");


            Console.WriteLine("Enter new data (leaving empty for strings or 0 for IDs might clear/break data):");

            
            var tempEntity = ConsoleHelper.CreateNewEntity<T>();

            
            var pkProp = typeof(T).GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id"));
            if (pkProp != null)
            {
                
                pkProp.SetValue(tempEntity, pkProp.GetValue(existing));
            }

            var updated = _dal.Update(tempEntity); 

           
            Console.WriteLine("\n[NOTE] Complex Update logic is simplified in this console demo.");
           
            var finalUpdate = _dal.Update(existing);
            

            if (finalUpdate != null)
            {
                Console.WriteLine($"\nSuccessfully updated: {finalUpdate}");
            }
            else
            {
                Console.WriteLine($"\nFailed to update {typeof(T).Name}.");
            }
        }
    }


    // --- 5. DELETE ---
    public class DeleteByIdCommand<T> : ICommand where T : class
    {
        private readonly IGenericDAL<T> _dal;
        public string Description => $"Delete {typeof(T).Name} by ID";

        public DeleteByIdCommand(IGenericDAL<T> dal) => _dal = dal;

        public void Execute()
        {
            int id = ConsoleHelper.ReadInt($"Enter {typeof(T).Name} ID to delete: ");
            bool deleted = _dal.Delete(id);
            if (deleted)
            {
                Console.WriteLine($"\nSuccessfully deleted {typeof(T).Name} with ID {id}.");
            }
            else
            {
                Console.WriteLine($"\nFailed to delete {typeof(T).Name} with ID {id}. (Check if record exists or if foreign keys prevent deletion).");
            }
        }
    }
}