global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.AspNetCore.JsonPatch;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using System.Linq.Expressions;
global using FluentValidation;

// ----------------------------------------
global using NoZeroDays.Api;
global using NoZeroDays.Api.Database;
global using NoZeroDays.Api.Enums;
global using NoZeroDays.Api.DTO;
global using NoZeroDays.Api.DTO.Habits;
global using NoZeroDays.Api.DTO.HabitTag;
global using NoZeroDays.Api.DTO.Tag;
global using NoZeroDays.Api.DTO.Common;
global using NoZeroDays.Api.DTO.User;
global using NoZeroDays.Api.DTO.Auth;
global using NoZeroDays.Api.Entities;
global using NoZeroDays.Api.Mapping.Mapperly;
global using NoZeroDays.Api.Mapping.Projections;
global using NoZeroDays.Api.Mapping.ManualMappings;
global using NoZeroDays.Api.Extensions;
global using NoZeroDays.Api.Middleware;
global using NoZeroDays.Api.Service.Sorting;
// ----------------------------------------
global using OpenTelemetry;
global using OpenTelemetry.Metrics;
global using OpenTelemetry.Resources;
global using OpenTelemetry.Trace;
// ----------------------------------------

global using Riok.Mapperly.Abstractions;
global using System.Linq.Dynamic.Core;

