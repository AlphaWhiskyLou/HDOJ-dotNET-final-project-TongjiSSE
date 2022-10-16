using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;

namespace AdminService
{
    public class AdminSampleService
    {
        private compilerContext _compilerContext = new compilerContext();

        public async Task<List<Sample>> GetSampleInfo()
        {
            var context = _compilerContext;
            IQueryable<Sample> samples =
                from sample in context.Samples select sample;
            return samples.ToList();

        }

        public async Task<bool> UpdateSample(Sample sample)
        {
            Sample sampleUpdate =
                _compilerContext.Samples.First(c => c.SampleId == sample.SampleId);

            sampleUpdate.IsTest = sample.IsTest;
            sampleUpdate.Input = sample.Input;
            sampleUpdate.Output = sample.Output;
            sampleUpdate.QuestionId = sample.QuestionId;

            _compilerContext.SaveChanges();


            return true;
        }

        public async Task<bool> DeleteQuestion(Sample sample)
        {
            Sample sampleDelete =
                 _compilerContext.Samples.First(c => c.SampleId == sample.SampleId);

            _compilerContext.Samples.Remove(sampleDelete);
            _compilerContext.SaveChanges();

            return true;
        }

        public async Task<bool> AddSample(Sample sample)
        {
            _compilerContext.Samples.Add(sample);
            _compilerContext.SaveChanges();

            return true;
        }
    }
}
