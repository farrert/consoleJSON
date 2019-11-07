using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_NTier_XmlJsonData.Models;
using Demo_NTier_XmlJsonData.DataAccessLayer;

namespace Demo_NTier_XmlJsonData.BusinessLayer
{
    /// <summary>
    /// Repository for CRUD
    /// Note: the _dataService object is instantiated with either the XML or Json class based on the value set in the DataConfig class
    /// </summary>
    class FlintstoneCharacterRepository : IFlintstoneCharacterRepository, IDisposable
    {
        private IDataService _dataService;
        List<FlintstoneCharacter> _characters;

        /// <summary>
        /// set the correct data service (XML or Json) data service and read in a list of all characters
        /// </summary>
        public FlintstoneCharacterRepository()
        {
            _dataService = SetDataService();
            _characters = _dataService.ReadAll() as List<FlintstoneCharacter>;
        }

        /// <summary>
        /// instantiate and return the correct data service (XML or Json) 
        /// </summary>
        /// <returns>data service object</returns>
        private IDataService SetDataService()
        {
            switch (DataConfig.dataType)
            {
                case DataType.XML:
                    return new DataServiceXml();
                    break;

                case DataType.JSON:
                    return new DataServiceJson();
                    break;

                default:
                    throw new Exception();
                    break;
            }
        }

        /// <summary>
        /// add a new character
        /// </summary>
        /// <param name="character">caracter</param>
        public void Add(FlintstoneCharacter character)
        {
            _characters.Add(character);
            _dataService.WriteAll(_characters);
        }

        /// <summary>
        /// delete a character
        /// </summary>
        /// <param name="id">character id</param>
        public void Delete(int id)
        {
            _characters.Remove(_characters.FirstOrDefault(c => c.Id == id));
            _dataService.WriteAll(_characters);
        }

        /// <summary>
        /// retrieve all characters
        /// </summary>
        /// <returns>all characters</returns>
        public IEnumerable<FlintstoneCharacter> GetAll()
        {
            return _characters;
        }

        /// <summary>
        /// retrieve a character by the id
        /// </summary>
        /// <param name="id">character id</param>
        /// <returns></returns>
        public FlintstoneCharacter GetById(int id)
        {
            return _characters.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// update a character
        /// </summary>
        /// <param name="character">character</param>
        public void Update(FlintstoneCharacter character)
        {
            _characters.Remove(_characters.FirstOrDefault(c => c.Id == character.Id));
            _characters.Add(character);
        }

        /// <summary>
        /// required if class will be use in a 'using" block
        /// </summary>
        public void Dispose()
        {
            _dataService = null;
            _characters = null;
        }
    }
}
