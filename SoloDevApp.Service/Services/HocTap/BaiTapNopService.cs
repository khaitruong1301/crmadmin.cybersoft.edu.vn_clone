using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

namespace SoloDevApp.Service.Services
{
    public interface IBaiTapNopService : IService<BaiTapNop, BaiTapNopViewModel>
    {
        Task<ResponseEntity> GetByUserIdAsync(string userId);

        Task<ResponseEntity> GetByClassAndUserIdAsync(int classId, string userId);
        Task<ResponseEntity> GetByExerciseIdAsync(int classId, int exerciseId);

        Task<ResponseEntity> ChamDiem(int id, BaiTapNopViewModel model);

        Task<ResponseEntity> LayBaiTapNopTheoLop(int classId);

        Task<int> KiemTraTrungBaiTap(string nguoiDungId, int maBaiTap);
        
    }

    public class BaiTapNopService : ServiceBase<BaiTapNop, BaiTapNopViewModel>, IBaiTapNopService
    {
        IBaiTapNopRepository _baiTapNopRepository;
        INguoiDungRepository _nguoiDungRepository;
        ILopHocRepository _lopHocRepository;
        IBaiTapRepository _baiTapRepository;

        ILopHocService _lopHocService;

        public BaiTapNopService(IBaiTapNopRepository baiTapNopRepository,
             INguoiDungRepository nguoiDungRepository,ILopHocRepository lopHocRepository,ILopHocService lopHocService,IBaiTapRepository baiTapRepository,
            IMapper mapper)
            : base(baiTapNopRepository, mapper)
        {
            _baiTapNopRepository = baiTapNopRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _lopHocRepository = lopHocRepository;
            _lopHocService = lopHocService;
            _baiTapRepository = baiTapRepository;
        }
        public async Task<int> KiemTraTrungBaiTap(string nguoiDungId, int maBaiTap)
        {
            List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
            columns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", nguoiDungId));
            columns.Add(new KeyValuePair<string, dynamic>("MaBaiTap", maBaiTap));

            BaiTapNop baiTapNop = await _baiTapNopRepository.GetSingleByListConditionAsync(columns);
            if (baiTapNop != null)
                return 1;
            return 0;
        }

        public async Task<ResponseEntity> LayBaiTapNopTheoLop(int classId)
        {
            try
            {
                IEnumerable<BaiTapNop> dsBaiTapNop = await _baiTapNopRepository.GetBaiTapNopTheoLop(classId);

                return new ResponseEntity(StatusCodeConstants.OK, dsBaiTapNop);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> ChamDiem(int id, BaiTapNopViewModel model)
        {
            try
            {
                BaiTapNop bt = await _baiTapNopRepository.GetSingleByIdAsync(id);
                bt.Diem = model.Diem;
                bt.NhanXet = model.NhanXet;
                await _baiTapNopRepository.UpdateAsync(id, bt);
                return new ResponseEntity(StatusCodeConstants.OK, bt, MessageConstants.UPDATE_SUCCESS);
                 


            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);

            }

        }

        public async Task<ResponseEntity> GetByClassAndUserIdAsync(int classId, string userId)
        {
            try
            {
                List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
                columns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", userId));
                columns.Add(new KeyValuePair<string, dynamic>("MaLopHoc", classId));

                IEnumerable<BaiTapNop> dsBaiTapNop = await _baiTapNopRepository.GetMultiByListConditionAndAsync(columns);
                List<BaiTapNopViewModel> dsBaiTapNopVm = _mapper.Map<List<BaiTapNopViewModel>>(dsBaiTapNop);

                return new ResponseEntity(StatusCodeConstants.OK, dsBaiTapNopVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetByExerciseIdAsync(int classId, int exerciseId)
        {
            try
            {
                 List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
                columns.Add(new KeyValuePair<string, dynamic>("MaLopHoc", classId));

                if (exerciseId !=0)
                {
                    columns.Add(new KeyValuePair<string, dynamic>("MaBaiTap", exerciseId));
                }

                IEnumerable<BaiTapNop> dsBaiTapNop = await _baiTapNopRepository.GetMultiByListConditionAndAsync(columns);
                List<BaiTapNopViewModel> dsBaiTapNopVm = _mapper.Map<List<BaiTapNopViewModel>>(dsBaiTapNop);

                if (exerciseId != 0)
                {
                    //Lấy danh sách tất cả người dùng thuộc lớp học này
                    BaiTap baiTap = await _baiTapRepository.GetSingleByIdAsync(exerciseId);

                    //Lấy ra danh sách học viên của lớp học đó
                    LopHoc lh = await _lopHocRepository.GetSingleByIdAsync(classId);
                    //HashSet<string> dsGiangVienMentorCu = JsonConvert.DeserializeObject<HashSet<string>>(lhHienTai.DanhSachGiangVien);
                    //HashSet<string> dsMentor = JsonConvert.DeserializeObject<HashSet<string>>(lhHienTai.DanhSachMentor);
                    HashSet<string> lstIdNguoiDung = JsonConvert.DeserializeObject<HashSet<string>>(lh.DanhSachHocVien);
                  
                    foreach(BaiTapNopViewModel bt in dsBaiTapNopVm)
                    {
                        DateTime dateDeadLine = lh.NgayBatDau.AddDays(baiTap.SoNgayKichHoat).AddDays(baiTap.HanNop);
                        int soNgayConLai = FuncUtilities.TinhKhoangCachNgay(dateDeadLine);
                      
                    }
               

                    foreach (var id in lstIdNguoiDung)
                    {
                        bool b = dsBaiTapNopVm.Any(x => x.MaNguoiDung == id);
                        if(!b)
                        {
                            NguoiDung nguoiDung = await _nguoiDungRepository.GetSingleByIdAsync(id);
                            BaiTapNopViewModel btNopVM = new BaiTapNopViewModel();
                  
                            btNopVM.Diem = 0;
                            DateTime dateDeadLine = lh.NgayBatDau.AddDays(baiTap.SoNgayKichHoat).AddDays(baiTap.HanNop);
                            int soNgayConLai = FuncUtilities.TinhKhoangCachNgay(dateDeadLine);
                        
                            btNopVM.MaBaiTap = baiTap.MaLoTrinh;
                            btNopVM.MaLopHoc = classId;
                            btNopVM.MaNguoiDung = id;
                            btNopVM.NoiDung = "";
                            btNopVM.TaiLieu = baiTap.TaiLieu;
                            dsBaiTapNopVm.Add(btNopVM);
                        }

                    }



                }
                return new ResponseEntity(StatusCodeConstants.OK, dsBaiTapNopVm);

            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetByUserIdAsync(string userId)
        {
            try
            {
                IEnumerable<BaiTapNop> dsBaiTapNop = await _baiTapNopRepository.GetMultiByConditionAsync("MaNguoiDung", userId);
                List<BaiTapNopViewModel> modelVm = _mapper.Map<List<BaiTapNopViewModel>>(dsBaiTapNop);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> InsertAsync(BaiTapNopViewModel modelVm)
        {
            try
            {
                BaiTapNop entity = _mapper.Map<BaiTapNop>(modelVm);
                await _baiTapNopRepository.InsertAsync(entity);

                modelVm = _mapper.Map<BaiTapNopViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.CREATED, modelVm, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> UpdateAsync(dynamic id, BaiTapNopViewModel modelVm)
        {
            try
            {
                List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
                columns.Add(new KeyValuePair<string, dynamic>("Id", id));
                columns.Add(new KeyValuePair<string, dynamic>("MaLopHoc", modelVm.MaLopHoc));
                columns.Add(new KeyValuePair<string, dynamic>("MaBaiTap", modelVm.MaBaiTap));
                columns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", modelVm.MaNguoiDung));

                BaiTapNop entity = await _baiTapNopRepository.GetSingleByListConditionAsync(columns);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, null, "Không tìm thấy tài nguyên!");

                entity = _mapper.Map<BaiTapNop>(modelVm);
                await _baiTapNopRepository.UpdateAsync(id, entity);

                modelVm = _mapper.Map<BaiTapNopViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}