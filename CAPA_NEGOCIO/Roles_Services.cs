using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class Roles_Services : IRoles_Services

    {

        private readonly IDataAccess sql;

        public Roles_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud

        //Metodo Get


        public async Task<IEnumerable<RolEntity>> Get()

        {

            try

            {

                var result = sql.QueryAsync<RolEntity>("sp_Rol_SelectAll");

                return await result;

            }

            catch (Exception)

            {

                throw;

            }


        }

        //Metodo GetById

        public async Task<RolEntity> GetById(RolEntity entity)

        {

            try

            {

                var result = sql.QueryFirstAsync<RolEntity>("sp_Rol_SelectById", new

                { entity.RolID });

                return await result;

            }

            catch (Exception)

            {

                throw;

            }

        }

        //Metodo Create

        public async Task<DBEntity> Create(RolEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Rol_Insert", new
            {
                entity.RolDescription,
                entity.RolType,
                entity.Status
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(RolEntity entity)

        {

            try

            {

                var result = sql.ExecuteAsync("sp_Rol_Update", new

                {

                    entity.RolID,

                    entity.RolDescription,

                    entity.Status,

                    entity.DateCreation

                    
                });

                return await result;

            }

            catch (Exception)

            {

                throw;

            }

        }

        //Metodo Delete

        public async Task<DBEntity> Delete(RolEntity entity)

        {

            try

            {

                var result = sql.ExecuteAsync("", new

                {

                    entity.RolID,


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
