using Microsoft.AspNetCore.Hosting;
using subrip.Models;
using subrip.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace subrip.Repository
{
    public class SubtitleRepository
    {
        private SubtitleContext _context;
        private IWebHostEnvironment _appEnvironment;

        public SubtitleRepository(SubtitleContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public void Add(Subtitle subtitle)
        {
            string caminho_WebRoot = _appEnvironment.WebRootPath;
            subtitle.Path = $@"{caminho_WebRoot}\Files\{subtitle.File.FileName}";
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    using (var fs = new FileStream(subtitle.Path, FileMode.OpenOrCreate))
                    {
                        subtitle.FileName = subtitle.File.FileName;
                        subtitle.File.CopyTo(fs);
                    }

                    _context.Add(subtitle);
                    _context.SaveChanges();
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw new Exception("Erro ao processar aquivo");
                }
            }

        }

        public void ProcessFile(Subtitle subtitle)
        {
            try
            {
                subtitle.Offset = subtitle.Offset.Replace(",", ".");
                TimeSpan offset = TimeSpan.Parse(subtitle.Offset);
                var read = File.ReadAllLines(subtitle.Path);
                var line = int.Parse(read[0]);

                for (int i = 0; i < read.Length; i++)
                {

                    if (read[i].Trim().CompareTo(line.ToString()) == 0)
                    {
                        line++;
                        read[i + 1] = read[i + 1].Replace("-->", "");
                        string valueOne = read[i + 1].Substring(0, 12).Replace(",", ".");
                        string valueTwo = read[i + 1].Substring(14, 12).Replace(",", ".");

                        TimeSpan timeOne = TimeSpan.Parse(valueOne);
                        TimeSpan TimeTwo = TimeSpan.Parse(valueTwo);

                        timeOne = timeOne.Add(offset);
                        TimeTwo = TimeTwo.Add(offset);

                        string format = @"hh\:mm\:ss\,fff";
                        read[i + 1] = $"{timeOne.ToString(format)} --> {TimeTwo.ToString(format)}";
                    }
                }
                File.WriteAllLines(subtitle.Path, read);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar aquivo");
            }

        }

        public List<Subtitle> GetAll()
        {
            return _context.Subtitles.ToList();
        }

        public Subtitle Get(int id)
        {
            return _context.Subtitles.Where(x => x.Id == id).FirstOrDefault();
        } 
    }
}
