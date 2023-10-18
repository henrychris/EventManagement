﻿using ErrorOr;

namespace EventModule.ServiceErrors;

public static class Errors
{
    public static class Event
    {
        public static Error NotFound => Error.NotFound(
            code: "Event.NotFound",
            description: "Event not found.");

        public static Error InvalidEventDate => Error.Validation(
            code: "Event.InvalidEventDate",
            description: "The event can't start in the past.");

        public static Error InvalidEventStartTime => Error.Validation(
            code: "Event.InvalidEventStartTime",
            description: "The event can't start before the registered date.");

        public static Error InvalidEventEndTime => Error.Validation(
            code: "Event.InvalidEventEndTime",
            description: "The event can't end before its date or start time.");

        public static Error InvalidTicketPrice => Error.Validation(
            code: "Event.InvalidTicketPrice",
            description: "The ticket price must be greater than zero.");

        // todo: add invalid capacity error
    }
}