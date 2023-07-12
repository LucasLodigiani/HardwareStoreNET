using AutoMapper;
using Common.Dtos;
using Common.Entities;
using Common.Enums;
using Data.DbContexts;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        public OrderService(IOrderRepository repository, IMapper mapper, ApplicationDbContext context, IProductRepository productRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
            _productRepository = productRepository;
        }

        public async Task<OrderViewDto> CreateOrder(OrderCreateDto orderDto)
        {

            var products = await _productRepository.GetProductsByIdsAsync(orderDto.ProductsId);
            if(products.Count <= 0)
            {
                throw new Exception("No se han cargado productos.");
            }
            //Descartar productos que esten pausados o archivados.
            products = products.Where(x => x.State == ProductState.Publicado).ToList();

            Order order = _mapper.Map<Order>(orderDto);
            order.Products = products;

            //Crear orden
            order.Id = await _repository.NewOrder(order);

            OrderViewDto newOrderViewDto = _mapper.Map<OrderViewDto>(order);

            return newOrderViewDto;

        }

        public async Task<List<OrderViewDto>> GetAllOrders()
        {
            List<Order> orders = await _repository.GetAllOrdersAsync();

            List<OrderViewDto> ordersViews = _mapper.Map<List<OrderViewDto>>(orders);

            return ordersViews;

        }

        public async Task<(int, OrderViewDto?)> UpdateOrder(OrderUpdateDto orderUpdateDto)
        {
            //TO DO: Filtrar productos que vienen de la db.
            Order order = await _repository.FindOrderByIdAsync(orderUpdateDto.Id);
            if(order == null)
            {
                return (0, null);
            }
            else if (order.UserId != orderUpdateDto.UserId)
            {
                //Retornar unauthorized
                return (2, null);
            }
            else if (order.State == OrderState.Vendido)
            {
                throw new Exception("El pedido ya ha sido vendido, no puede modificarse");
            }

            var products = await _productRepository.GetProductsByIdsAsync(orderUpdateDto.ProductsId);

            //TO DO: Arreglar que cuando se modifica un producto dos veces y se agrega el mismo id de producto, se duplica el producto en la orden.
            if (products.Count < 1)
            {
                throw new Exception("Debes agregar al menos 1 producto para modificar el pedido");
            }
            //Descartar productos que esten pausados o archivados.
            products = products.Where(x => x.State == ProductState.Publicado).ToList();


            _mapper.Map(orderUpdateDto, order);

            order.Products.AddRange(products);

            bool orderUpdated = await _repository.UpdateOrder(order);

            if( orderUpdated )
            {
                OrderViewDto orderView = _mapper.Map<OrderViewDto>(order);
                return (1, orderView);
            }
            else
            {
                throw new Exception("Ha ocurrido un error al modificar el pedido.");
            }

        }
    }
}
