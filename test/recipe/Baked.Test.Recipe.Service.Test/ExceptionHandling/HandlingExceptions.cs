﻿using Baked.ExceptionHandling;
using Baked.ExceptionHandling.ProblemDetails;
using Baked.Orm;
using Baked.Test.Orm;
using System.Net;

namespace Baked.Test.ExceptionHandling;

public class HandlingExceptions : TestServiceSpec
{
    [Test(Description = "Actual behaviour is not testable, this test is included only for documentation and to improve coverage")]
    public void HandledException_is_handled_by_default()
    {
        var exceptionsSamples = GiveMe.The<ExceptionSamples>();

        var task = () => exceptionsSamples.Throw(handled: true);

        task.ShouldThrow<TestServiceHandledException>();
    }

    [Test]
    public void HandledExceptionHandler_transforms_exception_to_ExceptionInfo()
    {
        var exception = new TestServiceHandledException("message");
        var handler = new HandledExceptionHandler();

        var actual = handler.Handle(exception);

        actual.ShouldBeOfType<ExceptionInfo>();
        actual.Exception.ShouldBe(exception);
        actual.Code.ShouldBe((int)HttpStatusCode.BadRequest);
        actual.Body.ShouldBe("message");
        actual.ExtraData.ShouldBeEmpty();
    }

    [Test]
    public void RecordNotFoundException_for_calls_primary_constructor_with_correct_mapping()
    {
        var recordNotFoundException = RecordNotFoundException.For<Entity>(field: "Id",
            value: GiveMe.AGuid("fadf")
        );

        recordNotFoundException.Message.ShouldBe("{0} with {1}: '{2}' does not exist");
        recordNotFoundException.ExtraData.ShouldContainKeys("name", "field", "value");
        recordNotFoundException.ExtraData.ValuesShouldBe("Entity", "Id", GiveMe.AGuid("fadf").ToString());
    }

    [Test]
    public void Query_throws_RecordNotFoundException_when_entity_with_given_id_is_not_found()
    {
        var entityQueryContext = GiveMe.The<IQueryContext<Entity>>();
        var task = () => entityQueryContext.SingleById(GiveMe.AGuid());

        task.ShouldThrow<RecordNotFoundException>();
    }

    [Test]
    public void RecordNotFoundException_status_code_is_bad_request()
    {
        var entityQueryContext = GiveMe.The<IQueryContext<Entity>>();
        var task = () => entityQueryContext.SingleById(GiveMe.AGuid());

        var actual = task.ShouldThrow<RecordNotFoundException>();

        actual.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public void RecordNotFoundException_status_code_can_be_overridden()
    {
        var entityQueryContext = GiveMe.The<IQueryContext<Entity>>();
        var task = () => entityQueryContext.SingleById(GiveMe.AGuid(), throwNotFound: true);

        var actual = task.ShouldThrow<RecordNotFoundException>();

        actual.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}