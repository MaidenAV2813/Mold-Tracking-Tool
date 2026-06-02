using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class Transaction_Services : ITransaction_Services

    {

        private readonly IDataAccess sql;

        public Transaction_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud

        //Metodo Get

        public async Task<IEnumerable<TransactionEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<TransactionEntity>("sp_Transactions_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<TransactionEntity> GetById(TransactionEntity entity)

        {
            try

            {
                var result = sql.QueryFirstAsync<TransactionEntity>("", new

                { entity.TransactionID });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo Create

        public async Task<DBEntity> Create(TransactionEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Transactions_Insert", new
            {
                entity.TransactionType,
                entity.TransactionStatus,
                entity.CreatedBy,
                entity.DateCreation,
                entity.DateModification,
                entity.ModifiedBy
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(TransactionEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Transactions_Update", new

                {
                    entity.TransactionID,
                    entity.TransactionType,
                    entity.TransactionStatus,
                    entity.DateModification,
                    entity.ModifiedBy
                });

                return await result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        //Metodo Delete
        public async Task<DBEntity> Delete(TransactionEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("", new
                {
                    entity.TransactionID,
                });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }

}
