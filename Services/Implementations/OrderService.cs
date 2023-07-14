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

        public async Task<OrderViewDto?> GetOrderById(int id, Guid userId)
        {
            Order order = await _repository.FindOrderByIdAsync(id);
            if (order == null)
            {
                return null;
            }
            else if(order.UserId != userId)
            {
                throw new Exception("No estas autorizado para ver este pedido.");
            }

            OrderViewDto orderView = _mapper.Map<OrderViewDto>(order);

            return orderView;

        }

        public async Task<Boolean> UpdateOrder(OrderUpdateDto orderUpdateDto)
        {
            Order order = await _repository.FindOrderByIdAsync(orderUpdateDto.Id);
            if(order == null)
            {
                return false;
            }
            else if (order.UserId != orderUpdateDto.UserId)
            {
                throw new Exception("No estas autorizado para modificar este pedido");
            }
            else if (order.State == OrderState.Vendido)
            {
                throw new Exception("El pedido ya ha sido vendido, no puede modificarse");
            }

            var products = await _productRepository.GetProductsByIdsAsync(orderUpdateDto.ProductsId);

            if (products.Count != 0)
            {
                //Descartar productos que esten pausados o archivados, y los productos que ya esten.
                products = products.Where(x => x.State == ProductState.Publicado).ToList();

                order.Products.AddRange(products.Except(order.Products));
            }
            
            _mapper.Map(orderUpdateDto, order);

            bool orderUpdated = await _repository.UpdateOrder(order);

            if(orderUpdated)
            {
                return true;
            }
            else
            {
                throw new Exception("Ha ocurrido un error al modificar el pedido.");
            }

        }
    }
}

