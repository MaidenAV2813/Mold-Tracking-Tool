using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class PartMaintenance_Services : IPartMaintenance_Services
    {
        private readonly IDataAccess sql;

        public PartMaintenance_Services(IDataAccess _sql)
        {
            sql = _sql;
        }

        public async Task<DBEntity> Create(PartMaintenanceEntity entity)
        {
            var result = sql.ExecuteAsync("sp_PartMaintenance_Insert", new
            {
                entity.OrderNum,
                entity.ItemNumberID,
                entity.LocationID,
                entity.QtyAsigned,
                entity.DateCreation,
                entity.DateModification,
                entity.CreatedBy,
                entity.ModifiedBy
            });

            return await result;
        }

        public async Task<IEnumerable<PartMaintenanceEntity>> Get(string orderNum)
        {
            var result = sql.QueryAsync<PartMaintenanceEntity>(
                "sp_PartMaintenance_List",
                new { OrderNum = orderNum });

            return await result;
        }

        public async Task<PartMaintenanceEntity> GetById(PartMaintenanceEntity entity)
        {
            var result = sql.QueryFirstAsync<PartMaintenanceEntity>(
                "sp_PartMaintenance_GetById",
                new { entity.PartMaintenanceID });

            return await result;
        }

        public async Task<DBEntity> Update(PartMaintenanceEntity entity)
        {
            var result = sql.ExecuteAsync("sp_PartMaintenance_Update", new
            {
                entity.PartMaintenanceID,
                entity.ItemNumberID,
                entity.LocationID,
                entity.QtyAsigned,
                entity.DateModification,
                entity.ModifiedBy
            });

            return await result;
        }

        public async Task<DBEntity> Delete(PartMaintenanceEntity entity)
        {
            var result = sql.ExecuteAsync(
                "sp_PartMaintenance_Delete",
                new
                {
                    entity.PartMaintenanceID,
                });

            return await result;
        }

        public async Task<IEnumerable<ItemBOHPartMaintenanceEntity>> GetItemBOH(ItemBOHPartMaintenanceEntity entity)
        {
            var result = sql.QueryAsync<ItemBOHPartMaintenanceEntity>(
                "sp_ItemBOH_PartMaintenance_Get",
                new
                {
                    entity.ItemNumberID
                });

            return await result;
        }
    }
}
