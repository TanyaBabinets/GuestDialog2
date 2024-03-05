using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace GuestDialog2.Models
{
     public class MesContext : DbContext
        {
        //Первые 3 записи отзывов представлены для примера. Для правильной работы кода нужно пройти регистрацию
        //    и затем войти под своим логином. При входе пользователя появляется кнопка "добавить отзыв".
        //    Под своим логином вы получаете доступ к редактированию и изменению только своих отзывов, чужих нет.
        //    3 кнопки появятся, если вы вошли и там уже есть ваш отзыв.
        //        Проблема возникла с отображением диалоговых окон "Детали" и "Удалить", их надо растянуть для правильного 
        //        отображения данных. Я так и не смогла найти где их настроить.
        //        Ну вроде все функции кода работают.


            public DbSet<Users> users { get; set; }
            public DbSet<Messages> messages { get; set; }
            public MesContext(DbContextOptions<MesContext> options)
             : base(options)

            {
                if (Database.EnsureCreated())
                {
                Users u1 = new Users { FirstName = "Tanya", LastName = "B", Login = "Tanya", Password = "111", Salt = "" };
                Users u2 = new Users { FirstName = "Mikhail", LastName = "Petrov", Login = "Misha", Password = "222", Salt = "" };
				Users u3 = new Users { FirstName = "Svetlana", LastName = "Svetova", Login = "Svetlana", Password = "333", Salt = "" };
                DateTime now = DateTime.Now;
                 
                    messages?.Add(new Messages
                    {

                        user = u1,
                        Datetime = DateTime.Now,
                        message = "Все отлично, спасибо большое",
                        mark = 9,

                    }); messages?.Add(new Messages
                    {

                        user = u2,
                        Datetime = DateTime.Now,
                        message = "Хорошая кухня, хотелось бы пиццу потоньше",
                        mark = 10,


                    });
                    messages?.Add(new Messages
                    {

                        user = u3,
                        Datetime = DateTime.Now,
                        message = "В ванной течет труба. Утром слышен шум от стройки",
                        mark = 3,


                    });
                    SaveChanges();
                }
            }
        }
    }






