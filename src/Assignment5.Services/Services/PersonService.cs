using Assignment5.Application.Interfaces;
using Assignment5.Domain.Entities;
using Assignment5.Domain.Enums;
using Assignment5.Domain.Interfaces;
using ClosedXML.Excel;

namespace Assignment5.Application.Services
{
    public class PersonService : IPersonService
    {
        public readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public void Create(Person person)
        {
            _personRepository.AddPerson(person);
        }

        public void Delete(Guid id)
        {
            _personRepository.DeletePerson(id);
        }

        public IEnumerable<Person> FilterMembersByBirthYear(int year, string comparisonType)
        {
            switch (comparisonType)
            {
                case "equal":
                    return _personRepository.GetAllPerson().Where(x => x.DateOfBirth.Year == year);

                case "greater":
                    return _personRepository.GetAllPerson().Where(x => x.DateOfBirth.Year > year);

                case "less":
                    return _personRepository.GetAllPerson().Where(x => x.DateOfBirth.Year < year);

                default:
                    return _personRepository.GetAllPerson();
            }
        }

        public IEnumerable<Person> GetAll()
        {
            return _personRepository.GetAllPerson().ToList();
        }

        public IEnumerable<Person> GetMaleMembers()
        {
            return _personRepository.GetAllPerson().Where(x => x.Gender == GenderType.Male);
        }

        public IEnumerable<string> GetMemberFullNames()
        {
            return _personRepository.GetAllPerson().Select(x => x.FirstName + " " + x.LastName);
        }

        public byte[] GetMembersAsExcelFile()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Members");
                var currentRow = 1;
                string[] headers = { "First Name", "Last Name", "Gender", "Date Of Birth", "Phone Number", "Birth Place", "Is Graduated" };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cell(currentRow, i + 1).Value = headers[i];
                    worksheet.Cell(currentRow, i + 1).Style
                    .Font.SetBold()
                             .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
                             .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(currentRow, i + 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                }

                foreach (var member in _personRepository.GetAllPerson())
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = member.FirstName;
                    worksheet.Cell(currentRow, 2).Value = member.LastName;
                    worksheet.Cell(currentRow, 3).Value = member.Gender.ToString();
                    worksheet.Cell(currentRow, 4).Value = member.DateOfBirth.ToString("yyyy-MM-dd");
                    worksheet.Cell(currentRow, 5).Value = member.PhoneNumber;
                    worksheet.Cell(currentRow, 6).Value = member.BirthPlace;
                    worksheet.Cell(currentRow, 7).Value = member.IsGraduated ? "Yes" : "No";

                    if (currentRow % 2 == 0)
                    {
                        worksheet.Range(currentRow, 1, currentRow, 7).Style
                                 .Fill.SetBackgroundColor(XLColor.LightGray);
                    }
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public Person GetOldestMember()
        {
            return _personRepository.GetAllPerson().OrderBy(x => x.DateOfBirth).FirstOrDefault();
        }

        public Person GetPersonById(Guid id)
        {
            return _personRepository.GetPersonById(id);
        }

        public void Update(Person person)
        {
            _personRepository.UpdatePerson(person);
        }
    }
}