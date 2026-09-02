using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class InventoryTransactions_Services : IInventoryTransactions_Services

    {

        private readonly IDataAccess sql;

        public InventoryTransactions_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud


        //Metodo Get

        public async Task<IEnumerable<InventoryTransactionsEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<InventoryTransactionsEntity>("sp_InventoryTransactions_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<InventoryTransactionsEntity> GetById(InventoryTransactionsEntity entity)

        {
            try

            {
                var result = sql.QueryFirstAsync<InventoryTransactionsEntity>("", new

                { entity.TransactionID });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo Create

        public async Task<DBEntity> Create(InventoryTransactionsEntity entity)
        {
            var result = sql.ExecuteAsync("sp_InventoryTransactions_Insert", new
            {
                entity.ItemNumberID,
                entity.TransactionTypeID,
                entity.LocationID,
                entity.Qty,
                entity.Comments,
                entity.DateCreation,
                entity.CreatedBy

            });

            return await result;
        }

        #endregion
    }

}
