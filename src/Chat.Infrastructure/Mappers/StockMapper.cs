using System;
using System.Linq;
using Chat.Domain.Domains;
using Chat.Infrastructure.RabbitMQ.Messages;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Chat.Infrastructure.Mappers
{
    public static class StockMapper
    {
        public static Stock ParseCsvToStock(this StockRequest stockRequest, string csv)
        {
            var csvParserOptions = new CsvParserOptions(false, ',');
            var csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
            var csvMapper = new StockMapping();
            var csvParser = new CsvParser<Stock>(csvParserOptions, csvMapper);

            var result = csvParser
                .ReadFromString(csvReaderOptions, csv)
                .First();

            return result?.Result ?? new Stock(stockRequest.StockCode, "N/D");
        }
    }

    internal class StockMapping : CsvMapping<Stock>
    {
        public StockMapping() : base()
        {
            MapProperty(0, x => x.Code);
            MapProperty(6, x => x.Amount);
        }
    }
}
