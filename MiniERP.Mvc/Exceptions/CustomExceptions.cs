using System;

namespace MiniERP.Mvc.Exceptions;

public class BadRequestException(string message) : Exception(message);

public class NotFoundException(string message) : Exception(message);