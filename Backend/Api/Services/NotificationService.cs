using Confluent.Kafka;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;

namespace Backend.Api.Services
{
    public class NotificationService
    {
        private readonly IConsumer<string, string> _kafkaConsumer;

        public NotificationService(IConsumer<string, string> kafkaConsumer)
        {
            _kafkaConsumer = kafkaConsumer;
        }

        public void StartListening()
        {
            _kafkaConsumer.Subscribe("order");

            while (true)
            {
                try
                {
                    var consumeResult = _kafkaConsumer.Consume(); // Читаем из Kafka
                    var message = System.Text.Json.JsonSerializer.Deserialize<OrderNotification>(consumeResult.Message.Value);

                    // Отправляем email
                    SendEmailNotification(message);
                }
                catch (Exception ex)
                {
                    // Логируем ошибки
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            }
        }

        private void SendEmailNotification(OrderNotification message)
        {
            var email = GetUserEmailByUserId(message.UserId); // Получить email по UserId

            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email not found; skipping notification.");
                return;
            }

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("E-Commerce System", "no-reply@ecommerce.com"));
            mailMessage.To.Add(new MailboxAddress("", email));
            mailMessage.Subject = "Order Confirmation";
            mailMessage.Body = new TextPart("plain")
            {
                Text = $"Your order with ID {message.OrderId} totaling {message.TotalPrice:C} has been created successfully!"
            };

            /*using (var client = new SmtpClient())
            {
                client.Connect("smtp.mailtrap.io", 587, false); // Настройте ваш SMTP
                client.Authenticate("your_smtp_username", "your_smtp_password");
                client.Send(mailMessage);
                client.Disconnect(true);
            }*/

            Console.WriteLine($"Email sent to {email} for order {message.OrderId}");
        }

        private string GetUserEmailByUserId(Guid userId)
        {
            // Заглушка: добавьте здесь логику получения email пользователя
            // Например, запрос к базе данных
            //return "user@example.com";
            return "artnotea04@gmail.com";
        }
    }

    public record OrderNotification
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public decimal TotalPrice { get; init; }
        public DateTimeOffset Timestamp { get; init; }
    }
}
