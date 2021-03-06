﻿using MailService.Common.Bus.Event;
using MailService.Contracts.Commands;
using MailService.Contracts.Events;
using MailService.Domain;
using MailService.Domain.Handlers;
using MailService.Domain.MailSenders;
using MailService.Domain.Repositories;
using MailService.Tests.Helpers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MailService.Tests.Handlers
{
    [TestFixture]
    public class SendPendingMailsCmdHandlerTests : AutoMockerTests
    {
        private Mock<IMailWriteRepository> _mailWriteRepositoryMock;
        private Mock<IMailSenderFacade> _mailSenderFacade;
        private Mock<IEventBus> _eventBusMock;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            _mailWriteRepositoryMock = Mocker.GetMock<IMailWriteRepository>();
            _mailSenderFacade = Mocker.GetMock<IMailSenderFacade>();
            _eventBusMock = Mocker.GetMock<IEventBus>();
        }

        [Test]
        public async Task Should_SaveChangesAndSendEvent()
        {
            //arrange
            var mb = new MailBuilder();
            var mails = new List<Mail> { mb.SetDefaults().Build(), mb.SetDefaults().Build() };

            _mailWriteRepositoryMock.Setup(x => x.GetPendingAsync(It.IsAny<int>())).ReturnsAsync(mails);
            var target = Mocker.CreateInstance<SendPendingMailsCmdHandler>();
            var request = new SendPendingMailsCmd();

            //act
            await target.Handle(request, CancellationToken.None);

            //assert
            _mailWriteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
            _eventBusMock.Verify(x => x.Publish(It.IsAny<SendingMailsCompletedEvent>()), Times.Once);
        }

    }
}
