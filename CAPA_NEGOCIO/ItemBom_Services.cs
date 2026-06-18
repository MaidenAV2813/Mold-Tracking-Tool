using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class ItemBom_Services : IItemBom_Services

    {

        private readonly IDataAccess sql;

        public ItemBom_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud


        //Metodo Get

        public async Task<IEnumerable<ItemBomEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<ItemBomEntity>("sp_ItemBom_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<ItemBomEntity> GetById(ItemBomEntity entity)

        {
            try

            {
                var result = sql.QueryFirstAsync<ItemBomEntity>("", new

                { entity.ItemNumberID });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo Create

        public async Task<DBEntity> Create(ItemBomEntity entity)
        {
            var result = sql.ExecuteAsync("sp_ItemBom_Insert", new
            {
                entity.MoldID,
                entity.ItemNumber,
                entity.ItemDescription,
                entity.ItemCost,
                entity.ItemInvMin,
                entity.ItemInvMax,
                entity.ItemSupplierNumber,
                entity.ActualSupplier,
                entity.UOM,
                entity.ItemStatus,
                entity.DateCreation,
                entity.DateModification,
                entity.CreatedBy,
                entity.ModifiedBy

            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(ItemBomEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_ItemBom_Update", new

                {
                    entity.ItemNumberID,
                    entity.MoldID,
                    entity.ItemNumber,
                    entity.ItemDescription,
                    entity.ItemCost,
                    entity.ItemInvMin,
                    entity.ItemInvMax,
                    entity.ItemSupplierNumber,
                    entity.ActualSupplier,
                    entity.UOM,
                    entity.ItemStatus,
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
        public async Task<DBEntity> Delete(ItemBomEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("", new
                {
                    entity.ItemNumberID,
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
