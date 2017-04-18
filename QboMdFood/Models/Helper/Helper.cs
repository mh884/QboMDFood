using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Exception;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.ReportService;

namespace QboMdFood.Models.Helper
{
    public class Helper
    {
        internal static T Add<T>(DataService service, T entity) where T : IEntity
        {


            T added = service.Add<T>(entity);

            return added;

        }

        internal static List<T> FindAll<T>(DataService service, T entity, int startPosition = 1, int maxResults = 100) where T : IEntity
        {


            ReadOnlyCollection<T> entityList = service.FindAll(entity, startPosition, maxResults);

            return entityList.ToList<T>();
        }

        internal static T FindById<T>(DataService service, T entity) where T : IEntity
        {

            T foundEntity = service.FindById(entity);

            return foundEntity;
        }



        internal static ReadOnlyCollection<IntuitBatchResponse> Batch<T>(ServiceContext context, Dictionary<OperationEnum, object> operationDictionary) where T : IEntity
        {
            DataService service = new DataService(context);
            List<T> addedList = new List<T>();
            List<T> newList = new List<T>();


            QueryService<T> entityContext = new QueryService<T>(context);

            Batch batch = service.CreateNewBatch();

            foreach (KeyValuePair<OperationEnum, object> entry in operationDictionary)
            {
                if (entry.Value.GetType().Name.Equals(typeof(T).Name))
                    batch.Add(entry.Value as IEntity, entry.Key.ToString() + entry.Value.GetType().Name, entry.Key);
                else
                    batch.Add(entry.Value as string, "Query" + typeof(T).Name);
            }



            batch.Execute();

            return batch.IntuitBatchItemResponses;
        }


        internal static T AddAsync<T>(ServiceContext context, T entity) where T : IEntity
        {
            //Initializing the Dataservice object with ServiceContext
            DataService service = new DataService(context);

            bool isAdded = false;

            IdsException exp = null;

            T actual = (T)Activator.CreateInstance(entity.GetType());
            // Used to signal the waiting test thread that a async operation have completed.    
            ManualResetEvent manualEvent = new ManualResetEvent(false);

            // Async callback events are anonomous and are in the same scope as the test code,    
            // and therefore have access to the manualEvent variable.    
            service.OnAddAsyncCompleted += (sender, e) =>
            {
                isAdded = true;
                manualEvent.Set();
                if (e.Error != null)
                {
                    exp = e.Error;
                }
                if (exp == null)
                {
                    if (e.Entity != null)
                    {
                        actual = (T)e.Entity;
                    }
                }
            };

            // Call the service method
            service.AddAsync(entity);

            manualEvent.WaitOne(30000, false); Thread.Sleep(10000);

            if (exp != null)
            {
                throw exp;
            }

            // Check if we completed the async call, or fail the test if we timed out.    
            if (!isAdded)
            {
                //return null;
            }

            // Set the event to non-signaled before making next async call.    
            manualEvent.Reset();

            return actual;

        }

        internal static List<T> FindAllAsync<T>(ServiceContext context, T entity, int startPosition = 1, int maxResults = 500) where T : IEntity
        {
            //Initializing the Dataservice object with ServiceContext
            DataService service = new DataService(context);

            bool isFindAll = false;

            IdsException exp = null;

            // Used to signal the waiting test thread that a async operation have completed.    
            ManualResetEvent manualEvent = new ManualResetEvent(false);

            List<T> entities = new List<T>();

            // Async callback events are anonomous and are in the same scope as the test code,    
            // and therefore have access to the manualEvent variable.    
            service.OnFindAllAsyncCompleted += (sender, e) =>
            {
                isFindAll = true;
                manualEvent.Set();
                if (e.Error != null)
                {
                    exp = e.Error;
                }
                if (exp == null)
                {
                    if (e.Entities != null)
                    {
                        foreach (IEntity en in e.Entities)
                        {
                            entities.Add((T)en);
                        }
                    }
                }
            };

            // Call the service method
            service.FindAllAsync<T>(entity, 1, 10);

            manualEvent.WaitOne(60000, false); Thread.Sleep(10000);


            //// Check if we completed the async call, or fail the test if we timed out.    
            //if (!isFindAll)
            //{
            //    return null;
            //}

            if (exp != null)
            {
                throw exp;
            }

            //if (entities != null)
            //{
            //    Assert.IsTrue(entities.Count >= 0);
            //}

            // Set the event to non-signaled before making next async call.    
            manualEvent.Reset();
            return entities;
        }

        internal static T FindByIdAsync<T>(ServiceContext context, T entity) where T : IEntity
        {
            //Initializing the Dataservice object with ServiceContext
            DataService service = new DataService(context);

            bool isFindById = false;

            IdsException exp = null;

            // Used to signal the waiting test thread that a async operation have completed.    
            ManualResetEvent manualEvent = new ManualResetEvent(false);

            T returnedEntity = default(T);

            // Async callback events are anonomous and are in the same scope as the test code,    
            // and therefore have access to the manualEvent variable.    
            service.OnFindByIdAsyncCompleted += (sender, e) =>
            {

                manualEvent.Set();
                isFindById = true;
                returnedEntity = (T)e.Entity;
            };

            // Call the service method
            service.FindByIdAsync<T>(entity);
            manualEvent.WaitOne(60000, false); Thread.Sleep(10000);

            //// Check if we completed the async call, or fail the test if we timed out.    
            //if (!isFindById)
            //{
            //    //return null;
            //}

            if (exp != null)
            {
                throw exp;
            }

            // Set the event to non-signaled before making next async call.    
            manualEvent.Reset();
            return returnedEntity;
        }






    }
}