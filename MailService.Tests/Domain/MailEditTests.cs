﻿using AutoMapper;
using FluentAssertions;
using MailService.Common.Exceptions;
using MailService.Contracts.Enums;
using MailService.Domain;
using MailService.Domain.Handlers;
using MailService.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailService.Tests.Domain
{
    [TestFixture]
    public class MailEditTests
    {
        private string _from, _body, _subject, _priority;
        private List<string> _to;
        private bool _isHtml;

        [SetUp]
        public void Setup()
        {
            _from = "from-after-edit@mail.com";
            _to = new List<string>() { "to-after-edit-1@mail.com", "to-after-edit-2@mail.com" };
            _subject = "Test subject after edit";
            _body = "Test body after edit";
            _isHtml = true;
            _priority = CustomMailPriority.High.ToString();
        }

        [Test]
        public void For_Editing_Should_EditAllProperties()
        {
            //arrange
            var mail = new MailBuilder().SetDefaults().Build();

            //act
            mail.Edit(_from, _to, _subject, _body, _isHtml, _priority);

            //assert
            mail.From.Should().Be(_from);
            mail.To.Should().BeEquivalentTo(_to);
            mail.Subject.Should().Be(_subject);
            mail.Body.Should().Be(_body);
            mail.IsHtml.Should().Be(_isHtml);
            mail.Priority.ToString().Should().Be(_priority);

        }

        [Test]
        public void For_EditingSentMail_Should_ThrowEx()
        {
            //arrange
            var mail = new MailBuilder().SetDefaults().Build();
            mail.MarkAsSent();

            //act
            Action act = () => mail.Edit(_from, _to, _subject, _body, _isHtml, _priority);

            //assert
            act.Should().Throw<AppException>();
        }

        [Test]
        public void For_EditingWithFromAndTo_Should_HavePendingStatus()
        {
            //arrange
            var mail = new MailBuilder().SetDefaults().Build();

            //act
            mail.Edit(_from, _to, _subject, _body, _isHtml, _priority);

            //assert
            mail.Status.Should().Be(MailStatus.Pending);
        }

        [Test]
        public void For_EditingWithEmptyFrom_Should_HaveDrafftStatus()
        {
            //arrange
            _from = null;
            var mail = new MailBuilder().SetDefaults().Build();

            //act
            mail.Edit(_from, _to, _subject, _body, _isHtml, _priority);

            //assert
            mail.Status.Should().Be(MailStatus.Draft);
        }

        [Test]
        public void For_EditingWithEmptyTo_Should_HaveDrafftStatus()
        {
            //arrange
            _to = new List<string>();
            var mail = new MailBuilder().SetDefaults().Build();

            //act
            mail.Edit(_from, _to, _subject, _body, _isHtml, _priority);

            //assert
            mail.Status.Should().Be(MailStatus.Draft);
        }
    }
}
