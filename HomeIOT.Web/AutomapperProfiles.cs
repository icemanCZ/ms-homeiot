using AutoMapper;
using HomeIOT.Data;
using HomeIOT.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Web
{
    public class SensorProfile : Profile
    {
        public SensorProfile()
        {
            CreateMap<Sensor, SensorViewModel>()
                .ForMember(svm => svm.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(svm => svm.Description, opt => opt.MapFrom(s => s.Description))
                .ForMember(svm => svm.Groups, opt => opt.MapFrom(s => s.Groups.Select(x => x.SensorGroupId)))
                .ForMember(svm => svm.AllGroups, opt => opt.ConvertUsing<AllGroupsValueConverter, ICollection<SensorInSensorGroup>>(s => s.Groups))
                .ForMember(svm => svm.Units, opt => opt.MapFrom(s => s.Units));
            CreateMap<SensorViewModel, Sensor>()
                .ForMember(s => s.Name, opt => opt.MapFrom(svm => svm.Name))
                .ForMember(s => s.Description, opt => opt.MapFrom(svm => svm.Description))
                .ForMember(s => s.Groups, opt => opt.MapFrom((svm, s) => UpdateGroups(svm, s)))
                .ForMember(s => s.Units, opt => opt.MapFrom(svm => svm.Units));
            CreateMap<Sensor, SensorDetailViewModel>()
                .ForMember(svm => svm.SensorId, opt => opt.MapFrom(s => s.SensorId))
                .ForMember(svm => svm.SensorName, opt => opt.MapFrom(s => s.Name))
                .ForMember(svm => svm.SensorDescription, opt => opt.MapFrom(s => s.Description))
                .ForMember(svm => svm.IsFavorited, opt => opt.MapFrom(s => s.IsFavorited))
                .ForMember(svm => svm.Units, opt => opt.MapFrom(s => s.Units));
        }

        private ICollection<SensorInSensorGroup> UpdateGroups(SensorViewModel source, Sensor destination)
        {
            if (destination.Groups != null || source.Groups != null)
            {
                destination.Groups = destination.Groups ?? new Collection<SensorInSensorGroup>();
                source.Groups = source.Groups ?? new List<int>();

                foreach (var toRemove in destination.Groups.Where(g => !source.Groups.Contains(g.SensorGroupId)).ToList())
                    destination.Groups.Remove(toRemove);

                foreach (var toAdd in source.Groups.Where(g => !destination.Groups.Any(x => x.SensorGroupId == g)).ToList())
                    destination.Groups.Add(new SensorInSensorGroup() { SensorId = destination.SensorId, SensorGroupId = toAdd });
            }

            return new Collection<SensorInSensorGroup>(destination.Groups.ToList());
        }

        private class AllGroupsValueConverter : IValueConverter<ICollection<SensorInSensorGroup>, SelectList>
        {
            private readonly DBContext _context;

            public AllGroupsValueConverter(DBContext context)
            {
                _context = context;
            }

            public SelectList Convert(ICollection<SensorInSensorGroup> sourceMember, ResolutionContext context)
            {
                return new SelectList(_context.SensorGroups, "SensorGroupId", "Name");
            }
        }
    }

    public class SensorDataProfile : Profile
    {
        public SensorDataProfile()
        {
            CreateMap<SensorData, SensorDataViewModel>()
                .ForMember(sdvm => sdvm.SensorId, opt => opt.MapFrom(sd => sd.SensorId))
                .ForMember(sdvm => sdvm.Value, opt => opt.MapFrom(sd => sd.Value))
                .ForMember(sdvm => sdvm.Timestamp, opt => opt.MapFrom(sd => sd.Timestamp));
        }
    }

    public class SensorGroupProfile : Profile
    {
        public SensorGroupProfile()
        {
            CreateMap<SensorGroup, SensorGroupViewModel>();
        }
    }

    public class ApplicationEventProfile : Profile
    {
        public ApplicationEventProfile()
        {
            CreateMap<ApplicationEvent, ApplicationEventViewModel>()
                .ForMember(evm => evm.ApplicationEventId, opt => opt.MapFrom(e => e.SensorId))
                .ForMember(evm => evm.EventType, opt => opt.MapFrom(e => e.EventType))
                .ForMember(evm => evm.Resolved, opt => opt.MapFrom(e => e.Resolved))
                .ForMember(evm => evm.ResolvedTimestamp, opt => opt.MapFrom(e => e.ResolvedTimestamp))
                .ForMember(evm => evm.Timestamp, opt => opt.MapFrom(e => e.Timestamp))
                .ForMember(evm => evm.SensorId, opt => opt.MapFrom(e => e.SensorId))
                .ForMember(evm => evm.SensorName, opt => opt.MapFrom(e => e.Sensor.Name))
                .ForMember(evm => evm.SensorUnits, opt => opt.MapFrom(e => e.Sensor.Units));
        }
    }
}