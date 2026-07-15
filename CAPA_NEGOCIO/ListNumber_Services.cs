using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class ListNumber_Services : IListNumber_Services

    {

        private readonly IDataAccess sql;

        public ListNumber_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud

        //Metodo Get
        public async Task<IEnumerable<ListNumberEntity>> Get()

        {
            try

            {
                var result = sql.QueryAsync<ListNumberEntity>("sp_ListNumber_List");

                return await result;

            }

            catch (Exception)

            {

                throw;
            }

        }

        //Metodo GetById

        public async Task<ListNumberEntity> GetById(ListNumberEntity entity)
        {
            var result = await sql.QueryFirstAsync<ListNumberEntity>(
                "sp_ListNumber_GetById",
                new
                {
                    ListNumberID = entity.ListNumberID
                });

            return result;
        }

        //Metodo Create

        public async Task<DBEntity> Create(ListNumberEntity entity)
        {
            var result = await sql.QueryFirstAsync<DBEntity>(
                "sp_ListNumber_Insert",
                new
                {
                    MoldID = entity.MoldID,
                    ListNumber = entity.ListNumber,
                    Description = entity.Description,
                    DateCreation = entity.DateCreation,
                    DateModification = entity.DateModification,
                    CreatedBy = entity.CreatedBy,
                    ModifiedBy = entity.ModifiedBy
                }
            );

            return result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(ListNumberEntity entity)
        {
            var result = await sql.QueryFirstAsync<DBEntity>(
                "sp_ListNumber_Update",
                new
                {
                    ListNumberID = entity.ListNumberID,
                    MoldID = entity.MoldID,
                    ListNumber = entity.ListNumber,
                    Description = entity.Description,
                    DateModification = entity.DateModification,
                    ModifiedBy = entity.ModifiedBy
                });

            return result;
        }

        //Metodo Delete

        public async Task<DBEntity> Delete(ListNumberEntity entity)
        {
            var result = await sql.QueryFirstAsync<DBEntity>(
                "sp_ListNumber_Delete",
                new
                {
                    ListNumberID = entity.ListNumberID,
                    ModifiedBy = entity.ModifiedBy
                });

            return result;
        }

        public async Task<IEnumerable<ListNumberEntity>> GetByMoldID(ListNumberEntity entity)
        {
            var result =
                await sql.QueryAsync<ListNumberEntity>("sp_ListNumber_GetByMoldID",new
                    {
                        MoldID = entity.MoldID
                    }
                );

            return result;
        }
        #endregion

    }


}
