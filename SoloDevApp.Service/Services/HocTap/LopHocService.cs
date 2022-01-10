using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface ILopHocService : IService<LopHoc, LopHocViewModel>
    {
        Task<ResponseEntity> GetInfoByIdAsync(dynamic id);

        Task<ResponseEntity> GetCourseByClassIdAsync(dynamic id);

        Task<ResponseEntity> GetByUserIdAsync(dynamic id);


        Task<ResponseEntity> InsertClassAsync(LopHocViewModel model);
        Task<ResponseEntity> UpdateClassAsync(int id,LopHocViewModel model);

        Task<ResponseEntity> DeleteByIdClassAsync(List<dynamic> Ids);

        Task<ResponseEntity> CheckSoLuongMentor();

        Task<ResponseEntity> GetClassByYear(int year);

        Task<ResponseEntity> GetListClassesByClassId(int classId);

        Task<ResponseEntity> AddClassesToClass(int classId, int classesId);

    }

    public class LopHocService : ServiceBase<LopHoc, LopHocViewModel>, ILopHocService
    {
        private ILopHocRepository _lopHocRepository;
        private INguoiDungRepository _nguoiDungRepository;
        private ILoTrinhRepository _loTrinhRepository;
        private IKhoaHocRepository _khoaHocRepository;
        private IBaiTapRepository _baiTapRepository;
        private IBaiTapNopRepository _baiTapNopRepository;
        private IDiemDanhRepository _diemDanhRepository;
        private ILopHoc_TaiLieuRepository _lopHoc_TaiLieuRepository;
        private IBuoiHocRepository _buoiHocRepository;
        private IBaiHoc_TaiLieu_Link_TracNghiemRepository _baiHoc_TaiLieu_Link_TracNghiemRepository;
        private IXemLaiBuoiHocRepository _xemLaiBuoiHocRepository;
        private IVideoExtraRepository _videoExtraRepository;
        private ITaiLieuBaiHocRepository _taiLieuBaiHocRepository;
        private ITaiLieuBaiTapRepository _taiLieuBaiTapRepository;
        private ITaiLieuDocThemRepository _taiLieuDocThemRepository;
        private ITaiLieuProjectLamThemRepository _taiLieuProjectLamThemRepository;
        private ITaiLieuCapstoneRepository _taiLieuCapstoneRepository;
        private ITracNghiemRepository _tracNghiemRepository;
     private IChuongHocRepository _chuongHocRepository;
        private IBaiHocRepository _baiHocRepository;
        private IBuoiHoc_NguoiDungRepository _buoiHocNguoiDungRepository;
        private IThongBaoRepository _thongBaoRepository;

        private readonly IAppSettings _appSettings;


        public LopHocService(ILopHocRepository lopHocRepository,
            INguoiDungRepository nguoiDungRepository,
            ILoTrinhRepository loTrinhRepository,
            IKhoaHocRepository khoaHocRepository,
            IBaiTapRepository baiTapRepository,
            IBaiTapNopRepository baiTapNopRepository,
            IDiemDanhRepository diemDanhRepository,
            ILopHoc_TaiLieuRepository lopHoc_TaiLieuRepository,
            IBuoiHocRepository buoiHocRepository,
            IBaiHoc_TaiLieu_Link_TracNghiemRepository baiHoc_TaiLieu_Link_TracNghiemRepository,
            IXemLaiBuoiHocRepository xemLaiBuoiHocRepository,
            IVideoExtraRepository videoExtraRepository,
            ITaiLieuBaiHocRepository taiLieuBaiHocRepository,
            ITaiLieuBaiTapRepository taiLieuBaiTapRepository,
            ITaiLieuDocThemRepository taiLieuDocThemRepository,
            ITaiLieuProjectLamThemRepository taiLieuProjectLamThemRepository,
            ITaiLieuCapstoneRepository taiLieuCapstoneRepository,
            ITracNghiemRepository tracNghiemRepository,
            IChuongHocRepository chuongHocRepository,
            IBaiHocRepository baiHocRepository,
            IBuoiHoc_NguoiDungRepository buoiHoc_NguoiDungRepository,
            IThongBaoRepository thongBaoRepository,
        IAppSettings appSettings,
        IMapper mapper)
            : base(lopHocRepository, mapper)
        {
            _lopHocRepository = lopHocRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _loTrinhRepository = loTrinhRepository;
            _khoaHocRepository = khoaHocRepository;
            _baiTapRepository = baiTapRepository;
            _baiTapNopRepository = baiTapNopRepository;
            _diemDanhRepository = diemDanhRepository;
            _lopHoc_TaiLieuRepository = lopHoc_TaiLieuRepository;
            _buoiHocRepository = buoiHocRepository;
            _baiHoc_TaiLieu_Link_TracNghiemRepository = baiHoc_TaiLieu_Link_TracNghiemRepository;
            _xemLaiBuoiHocRepository = xemLaiBuoiHocRepository;
            _videoExtraRepository = videoExtraRepository;
            _taiLieuBaiHocRepository = taiLieuBaiHocRepository;
            _taiLieuBaiTapRepository = taiLieuBaiTapRepository;
            _taiLieuDocThemRepository = taiLieuDocThemRepository;
            _taiLieuProjectLamThemRepository = taiLieuProjectLamThemRepository;
            _taiLieuCapstoneRepository = taiLieuCapstoneRepository;
            _tracNghiemRepository = tracNghiemRepository;
            _chuongHocRepository = chuongHocRepository;
            _baiHocRepository = baiHocRepository;
            _buoiHocNguoiDungRepository = buoiHoc_NguoiDungRepository;
            _thongBaoRepository = thongBaoRepository;
            _appSettings = appSettings;

        }

        private string GenerateToken(LopHocViewModel lopHoc)
        {
            try
            {


                DateTime setNgayBatDau = DateTime.ParseExact(lopHoc.NgayBatDau, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                DateTime setNgayKetThuc = DateTime.ParseExact(lopHoc.NgayKetThuc, "dd-MM-yyyy", CultureInfo.InvariantCulture);

               
                setNgayKetThuc = setNgayKetThuc.AddMonths(5);

                DateTime baseDate = new DateTime(1970, 1, 1);
                TimeSpan diff = setNgayKetThuc - baseDate;


                var arrInfo = new List<Claim> {
                new Claim("tenLop", lopHoc.TenLopHoc),
                new Claim("HetHanString", setNgayKetThuc.ToString("dd/MM/yyyy")),
                new Claim("HetHanTime",diff.TotalMilliseconds.ToString())


            };

                SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Secret));
                var token = new JwtSecurityToken(
                            claims: arrInfo,
                            notBefore: setNgayBatDau,
                            expires: setNgayKetThuc.AddDays(2),
                            signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<ResponseEntity> GetClassByYear(int year)
        {
            try
            {

                IEnumerable<LopHoc> dsLopHoc = await _lopHocRepository.GetClassByYear(year);
                
                return new ResponseEntity(StatusCodeConstants.OK, dsLopHoc);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }


        public async Task<ResponseEntity> CheckSoLuongMentor()
        {
            try
            {
                DateTime dNow = DateTime.Now;

               // IEnumerable<LopHoc> lstLopHoc = await _lopHocRepository.GetAllAsync();
               // List<LopHoc> lstQuaHan = new List<LopHoc>();

               // //loc lop hoc qua 2 thang
               //foreach (LopHoc item in lstLopHoc)
               // {
               //     double checkSoNgay = Math.Round((dNow - item.NgayKetThuc).TotalDays);

               //     if (checkSoNgay > 60)
               //         lstQuaHan.Add(item);
               // }

               // // kiem tra va xoa tai lieu cua lop 2 thang
               // foreach (LopHoc item in lstQuaHan)
               // {
               //     IEnumerable<LopHoc_TaiLieu> lstLopTaiLieu = await _lopHoc_TaiLieuRepository.GetTheoMaLop(item.Id);

               //     lstLopTaiLieu = lstLopTaiLieu.Where(n => n.MaBaiTap == 0);

               //     if (lstLopTaiLieu.Count() > 0)
               //     {
               //         foreach (var itemLopTL in lstLopTaiLieu)
               //         {

               //             if (itemLopTL.MaBaiTap == 0)
               //             {
               //                 dynamic duongDan = JsonConvert.DeserializeObject(itemLopTL.DuongDan);
               //                 string fileUrl = duongDan[1];

               //                 string pathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
               //                 var path = @pathFolder + @fileUrl;

               //                 if (System.IO.File.Exists(path))
               //                 {
               //                     System.IO.File.Delete(path);
               //                 }

               //             }
               //         }

               //     }
               // }

                // lay danh sach lop
                IEnumerable<LopHoc> dsLopHoc = await _lopHocRepository.GetAllAsync();

                dsLopHoc = dsLopHoc.Where(n => n.MaTrangThai == 2 && n.NgayBatDau < dNow && dNow < n.NgayKetThuc);

                List<string> lstLopWarn = new List<string>();

                foreach (LopHoc item in dsLopHoc)
                {
                    double date =Math.Round((dNow - item.NgayBatDau).TotalDays);

                    if(date > 30)
                    {
                        //lay danh sach diem danh theo lop, lay ngay diem danh gan nhat
                        IEnumerable<DiemDanh> dsDiemDanh = await _diemDanhRepository.GetTheoMaLop(item.Id);

                        if(dsDiemDanh.Count() > 0)
                        {
                            DiemDanh diemDanh = dsDiemDanh.OrderByDescending(n => n.NgayTao).FirstOrDefault();

                            List<string> dsDiemDanhHocVien = JsonConvert.DeserializeObject<List<string>>(diemDanh.DanhSachHocVien);

                            List<string> dsMentor = JsonConvert.DeserializeObject<List<string>>(item.DanhSachMentor);

                            int slDiemDanh = dsDiemDanhHocVien.Count();

                            int slMentor = dsMentor.Count();

                            // > 35  3 mentor , 20 < 2 monter <35 , 1 mentor <20
                            if (slDiemDanh <= 23 && slDiemDanh >= 33)
                            {
                                if(slMentor >= 3)
                                {
                                    LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(item.Id);
                                    lstLopWarn.Add(lopHoc.TenLopHoc);
                                }
                            }

                            //if (slDiemDanh < 15)
                            if (slDiemDanh < 23 )
                            {
                                if (slMentor >= 2)
                                {
                                    LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(item.Id);
                                    lstLopWarn.Add(lopHoc.TenLopHoc);
                                }
                            }
                        }

                    }
                    
                }



                return new ResponseEntity(StatusCodeConstants.OK, lstLopWarn);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetByUserIdAsync(dynamic id)
        {
            try
            {
                NguoiDung nguoiDung = await _nguoiDungRepository.GetSingleByIdAsync(id);
                if (nguoiDung == null || nguoiDung.DanhSachLopHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                List<LopHocViewModel> dsLopHocVm = new List<LopHocViewModel>();
                if (nguoiDung.DanhSachLopHoc != null)
                {
                    List<dynamic> dsMaLopHoc = JsonConvert.DeserializeObject<List<dynamic>>(nguoiDung.DanhSachLopHoc);
                    var dsLopHoc = await _lopHocRepository.GetMultiByListIdAsync(dsMaLopHoc);
                    dsLopHocVm = _mapper.Map<List<LopHocViewModel>>(dsLopHoc);
                }
                return new ResponseEntity(StatusCodeConstants.OK, dsLopHocVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetCourseByClassIdAsync(dynamic id)
        {
            try
            {
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(id);
                if (lopHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                // LẤY DANH SÁCH KHÓA HỌC ĐÃ KÍCH HOẠT
                List<KhoaHoc> dsKhoaHocKichHoat = await GetCourseActived(lopHoc.MaLoTrinh, lopHoc.NgayBatDau);
                var dsKhoaHocVm = _mapper.Map<List<KhoaHocViewModel>>(dsKhoaHocKichHoat);

                // LẤY DANH SÁCH BÀI TẬP ĐÃ KÍCH HOẠT
                IEnumerable<BaiTap> dsBaiTap = await _baiTapRepository.GetMultiByConditionAsync("MaLoTrinh", lopHoc.MaLoTrinh);
                List<BaiTapViewModel> dsBaiTapVm = _mapper.Map<List<BaiTapViewModel>>(dsBaiTap);
                //dsBaiTapVm = await GetExerciseActived(dsBaiTapVm, dsKhoaHocKichHoat, lopHoc.NgayBatDau);

                var modelVm = new KhoaHocKichHoatViewModel()
                {
                    DanhSachKhoaHoc = _mapper.Map<List<KhoaHocViewModel>>(dsKhoaHocKichHoat),
                    DanhSachBaiTap = dsBaiTapVm
                };

                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetInfoByIdAsync(dynamic id)
        {
            HashSet<dynamic> listId = new HashSet<dynamic>();
            try
            {
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(id);
                if (lopHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                ThongTinLopHocViewModel thongTinLopHocVm = _mapper.Map<ThongTinLopHocViewModel>(lopHoc);
             /*   foreach (dynamic item in thongTinLopHocVm.DanhSachGiangVien)
                {
                    listId.Add(item);
                }
                foreach (dynamic item in thongTinLopHocVm.DanhSachMentor)
                {
                    listId.Add(item);
                }*/
                foreach (dynamic item in thongTinLopHocVm.DanhSachHocVien)
                {
                    listId.Add(item);
                }

                // LẤY DANH SÁCH HỌC VIÊN
                var dsNguoiDung = await _nguoiDungRepository.GetMultiByIdAsync(listId.ToList());
                thongTinLopHocVm.DanhSachNguoiDung = _mapper.Map<List<NguoiDungViewModel>>(dsNguoiDung);

                // LẤY DANH SÁCH KHÓA HỌC ĐÃ KÍCH HOẠT
                List<KhoaHoc> dsKhoaHocKichHoat = await GetCourseActived(lopHoc.MaLoTrinh, lopHoc.NgayBatDau);
                thongTinLopHocVm.DanhSachKhoaHoc = _mapper.Map<List<KhoaHocViewModel>>(dsKhoaHocKichHoat);

                return new ResponseEntity(StatusCodeConstants.OK, thongTinLopHocVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

      

        private async Task<List<KhoaHoc>> GetCourseActived(int maLoTrinh, DateTime ngayBatDau)
        {
            LoTrinh loTrinh = await _loTrinhRepository.GetSingleByIdAsync(maLoTrinh);

            List<KhoaHoc> dsKhoaHoc = new List<KhoaHoc>();
            if (!string.IsNullOrEmpty(loTrinh.DanhSachKhoaHoc))
            {
                // Convert string json thành mảng
                List<dynamic> dsMaKhoaHoc = JsonConvert.DeserializeObject<List<dynamic>>(loTrinh.DanhSachKhoaHoc);
                // Lấy danh sách khóa học theo mảng id
                var listKhoaHoc = await _khoaHocRepository.GetMultiByIdAsync(dsMaKhoaHoc);
                // Sắp xếp đúng thứ tự
                foreach (dynamic idKhoaHoc in dsMaKhoaHoc)
                {
                    dsKhoaHoc.Add(listKhoaHoc.FirstOrDefault(x => x.Id == idKhoaHoc));
                }
            }

            // KIỂM TRA XEM ĐẾN NGÀY KÍCH HOẠT CHƯA
            //int demNgay = 0;
            //int soNgayTuLucKhaiGiang = FuncUtilities.TinhKhoangCachNgay(ngayBatDau);



            List<KhoaHoc> dsKhoaHocKichHoat = new List<KhoaHoc>();
            foreach (KhoaHoc item in dsKhoaHoc)
            {
                //TimeSpan time
                //Nếu là khóa học kích hoạt sẵn cho học viên
                //Ngay kich hoat khoa hoc
                //Tinh khoang cach ngay hien hanh va ngay do

                if (item.KichHoatSan )
                {
                    dsKhoaHocKichHoat.Add(item);
                    ////Tinh1 khoang cach tu ngay do den ngay hien hanh
                    //DateTime ngayKichHoatKhDauTien = ngayBatDau.AddDays(item.SoNgayKichHoat).AddDays(7);
                    //DateTime ngayKetThuc = ngayKichHoatKhDauTien.AddDays(Setting.thoiGianHoc);//36 ngày

                    //TimeSpan ts = DateTime.Now.Date - ngayKichHoatKhDauTien.Date ;

                    //if (ts.Days <= Setting.thoiGianHoc && ngayKetThuc >= DateTime.Now.Date)
                    //{
                    //    dsKhoaHocKichHoat.Add(item);
                    //}
                }
                else 
                {
                    DateTime ngayKichHoat = ngayBatDau.AddDays(item.SoNgayKichHoat).AddDays(-7);
                    DateTime ngayKetThuc = ngayKichHoat.AddDays(Setting.thoiGianHoc);//36 ngày
                    

                    TimeSpan ts =  DateTime.Now.Date - ngayKichHoat.Date;

                    if(ts.Days <= Setting.thoiGianHoc && ts.Days > 0 && ngayKetThuc >= DateTime.Now.Date )
                    {
                        dsKhoaHocKichHoat.Add(item);

                    }
                    //demNgay += item.SoNgayKichHoat;
                    // Kiểm tra xem đã đến ngày kích hoạt chưa
                    // Mở khóa học mới trước 7 ngày (1 tuần) cho học viên
                    //if (demNgay > (soNgayTuLucKhaiGiang - 7))
                    //{
                    //    break;
                    //}
                    //else
                    //{
                    //}
                }


            }
            return dsKhoaHocKichHoat;
        }

        private async Task<List<BaiTapViewModel>> GetExerciseActived(List<BaiTapViewModel> dsBaiTap, List<KhoaHoc> dsKhoaHoc, DateTime ngayBatDau)
        {
            List<BaiTapViewModel> dsBaiTapVm = new List<BaiTapViewModel>();

            // KIỂM TRA XEM ĐẾN NGÀY KÍCH HOẠT CHƯA
            //int soNgayTuLucKhaiGiang = FuncUtilities.TinhKhoangCachNgay(ngayBatDau);
            if (dsKhoaHoc.Count > 0)
            {

                foreach (BaiTapViewModel baiTap in dsBaiTap)
                {
                    DateTime ngayKichHoat = ngayBatDau.AddDays(baiTap.SoNgayKichHoat);
                    DateTime ngayHanNop = ngayKichHoat.AddDays(baiTap.SoNgayKichHoat).AddDays(baiTap.HanNop);

                    TimeSpan ts = ngayKichHoat.Date - DateTime.Now.Date;

                    if (ts.Days<=baiTap.SoNgayKichHoat)
                    {
                        TimeSpan tsHanNop = ngayHanNop - DateTime.Now.Date;
                        if (ngayKichHoat>DateTime.Now.Date )
                        {
                            continue;
                        }
                        if (tsHanNop.TotalDays <= baiTap.HanNop)
                        {
                            baiTap.HanNop = tsHanNop.Days;
                            baiTap.TrangThai = true;
                        }
                        else
                        {
                            baiTap.TrangThai = false;
                        }
                        dsBaiTapVm.Add(baiTap);

                    }
                }
                        //soNgayKichHoat += baiTap.SoNgayKichHoat;
                        //// NẾU TỔNG SỐ NGÀY KÍCH HOẠT NHỎ HƠN SỐ NGÀY TÍNH TỪ LÚC KHAI GIẢNG ĐẾN HÔM NAY
                        //if (soNgayKichHoat < soNgayTuLucKhaiGiang)
                        //{
                        //    // HIỂN THỊ CHO HỌC VIÊN NHƯNG BÁO LÀ ĐÃ HẾT HẠN NỘP
                        //    baiTap.TrangThai = false;
                        //    dsBaiTapVm.Add(baiTap);
                        //}
                        //// NẾU TỔNG SỐ NGÀY KÍCH HOẠT BẰNG SỐ NGÀY TÍNH TỪ LÚC KHAI GIẢNG ĐẾN HÔM NAY
                        //// HOẶC NẾU LỚN HƠN NHƯNG KHÔNG VƯỢT QUÁ SỐ NGÀY KÍCH HOẠT CỦA BÀI TẬP ĐÓ
                        //else if (soNgayKichHoat == soNgayTuLucKhaiGiang || (soNgayKichHoat - soNgayTuLucKhaiGiang) <= baiTap.SoNgayKichHoat)
                        //{
                        //    // HIỂN THỊ CHO HỌC VIÊN VÀ VẪN CHO NỘP BÀI
                        //    baiTap.TrangThai = true;
                        //    dsBaiTapVm.Add(baiTap);
                        //}
                        //else
                        //{
                        //    break;
                        //}
            }
            return dsBaiTapVm;
        }

   
   
      
        public async Task<ResponseEntity> InsertClassAsync(LopHocViewModel model)
        {

            model.DanhSachHocVienNew = "[]";
            model.Token = GenerateToken(model);

            var result = await InsertAsync(model);
            LopHoc lhHienTai = await _lopHocRepository.GetSingleByConditionAsync("BiDanh", model.BiDanh);

            //Lấy ra danh sách mentor và người dùng thêm lớp học vào 

            List<dynamic> dsMentorGiangVien = new List<dynamic>();
            dsMentorGiangVien.AddRange(model.DanhSachGiangVien);
            dsMentorGiangVien.AddRange(model.DanhSachMentor);
            foreach(dynamic id in dsMentorGiangVien)
            {
                //Bổ sung lớp dô mảng lớp
                NguoiDung nd = await _nguoiDungRepository.GetSingleByIdAsync(id);
                //Lấy ra lớp học
                List<dynamic> dsLopHoc = JsonConvert.DeserializeObject<List<dynamic>>(nd.DanhSachLopHoc);
                //Lấy lớp học hiện tại

                dsLopHoc.Add(lhHienTai.Id);
                nd.DanhSachLopHoc = JsonConvert.SerializeObject(dsLopHoc);
                await _nguoiDungRepository.UpdateAsync(nd.Id, nd);
            }

            return result;



        }

        public async Task<ResponseEntity> UpdateClassAsync(int id, LopHocViewModel model)
        {
           

                LopHoc lhHienTai = await _lopHocRepository.GetSingleByIdAsync(id);
                //Tìm những thằng cũ remove ra
                HashSet<string> dsGiangVienMentorCu = JsonConvert.DeserializeObject<HashSet<string>>(lhHienTai.DanhSachGiangVien);
                HashSet<string> dsMentor = JsonConvert.DeserializeObject<HashSet<string>>(lhHienTai.DanhSachMentor);
                dsGiangVienMentorCu.ToList().AddRange(dsMentor);

                foreach (dynamic idNguoiDung in dsGiangVienMentorCu)
                {
                    NguoiDung nd = await _nguoiDungRepository.GetSingleByIdAsync(idNguoiDung);
                    //Lấy ra lớp học
                    List<dynamic> dsLopHoc = JsonConvert.DeserializeObject<List<dynamic>>(nd.DanhSachLopHoc);
                    //Lấy lớp học hiện tại
                    dsLopHoc.Remove(idNguoiDung);
                    nd.DanhSachLopHoc = JsonConvert.SerializeObject(dsLopHoc);
                    await _nguoiDungRepository.UpdateAsync(nd.Id, nd);

                }

                //Tìm thằng mới add vô
                //Lấy ra danh sách mentor và người dùng thêm lớp học vào 
                List<dynamic> dsMentorGiangVien = new List<dynamic>();
                dsMentorGiangVien.AddRange(model.DanhSachGiangVien);
                dsMentorGiangVien.AddRange(model.DanhSachMentor);
                foreach (dynamic idMentorGiangVien in dsMentorGiangVien)
                {
                    //Bổ sung lớp dô mảng lớp
                    NguoiDung nd =  await  _nguoiDungRepository.GetSingleByIdAsync(idMentorGiangVien);
                    //Lấy ra lớp học
                    List<dynamic> dsLopHoc = JsonConvert.DeserializeObject<List<dynamic>>(nd.DanhSachLopHoc);
                    //Lấy lớp học hiện tại

                    dsLopHoc.Add(lhHienTai.Id);
                    nd.DanhSachLopHoc = JsonConvert.SerializeObject(dsLopHoc);
                    await _nguoiDungRepository.UpdateAsyncHasArrayNull(nd.Id, nd);
                }

            model.Token = GenerateToken(model);
            var result = await UpdateAsyncHasArrayNull(id, model);

            return result;
        }

        public async Task<ResponseEntity> DeleteByIdClassAsync(List<dynamic> Ids)
        {
            var result = await DeleteByIdAsync(Ids);
            foreach (dynamic id in Ids)
            {
                LopHoc lhHienTai = await _lopHocRepository.GetSingleByIdAsync(id);
                //Tìm những thằng cũ remove ra
                HashSet<string> dsGiangVienMentorCu = JsonConvert.DeserializeObject<HashSet<string>>(lhHienTai.DanhSachGiangVien);
                HashSet<string> dsMentor = JsonConvert.DeserializeObject<HashSet<string>>(lhHienTai.DanhSachMentor);
                dsGiangVienMentorCu.ToList().AddRange(dsMentor);

                foreach (dynamic idNguoiDung in dsGiangVienMentorCu)
                {
                    NguoiDung nd = await _nguoiDungRepository.GetSingleByIdAsync(idNguoiDung);
                    //Lấy ra lớp học
                    List<dynamic> dsLopHoc = JsonConvert.DeserializeObject<List<dynamic>>(nd.DanhSachLopHoc);
                    //Lấy lớp học hiện tại
                    dsLopHoc.Remove(idNguoiDung);
                    nd.DanhSachLopHoc = JsonConvert.SerializeObject(dsLopHoc);
                    await _nguoiDungRepository.UpdateAsyncHasArrayNull(nd.Id, nd);

                }
            }
            return result;
        }


        public async Task<ResponseEntity> GetListClassesByClassId(int classId)
        {
            try
            {
                string maNguoiDung = "d9699b77-f003-42c4-b050-26607079a789";

                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(classId);

                if (lopHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                //Lấy tất cả thông báo của người dùng
                ThongBao thongBao = await _thongBaoRepository.GetSingleByConditionAsync("MaNguoiDung", maNguoiDung);

                List<NoiDungThongBao> lsNoiDungThongBaoVm = new List<NoiDungThongBao>();

                if (thongBao != null)
                {
                    IEnumerable<NoiDungThongBao> lsNoiDungThongBao = JsonConvert.DeserializeObject<IEnumerable<NoiDungThongBao>>(thongBao.NoiDung);

                    lsNoiDungThongBaoVm = lsNoiDungThongBao.ToList();
                }

                //Lấy thông tin của lớp học

                ThongTinLopHocBySkill thongTinLopHoc = new ThongTinLopHocBySkill();

                thongTinLopHoc.TenLopHoc = lopHoc.TenLopHoc;
                thongTinLopHoc.BiDanh = lopHoc.BiDanh;
                thongTinLopHoc.NgayBatDau = FuncUtilities.ConvertDateToString(lopHoc.NgayBatDau);
                thongTinLopHoc.NgayKetThuc = FuncUtilities.ConvertDateToString(lopHoc.NgayKetThuc);
                thongTinLopHoc.SoHocVien = lopHoc.SoHocVien;
                thongTinLopHoc.ThoiKhoaBieu = lopHoc?.ThoiKhoaBieu;
                thongTinLopHoc.Token = lopHoc?.Token;

                List<dynamic> danhSachBuoi = JsonConvert.DeserializeObject<List<dynamic>>(lopHoc.DanhSachBuoi);

                IEnumerable<BuoiHoc> dsBuoiHoc = await _buoiHocRepository.GetMultiByIdAsync(danhSachBuoi);

                List<BuoiHocBySkillViewModel> dsBuoiHocBySkillVm = new List<BuoiHocBySkillViewModel>();

                //Lấy ra danh sách các buoihoc_nguoidung từ danh sách buổi
                IEnumerable<BuoiHoc_NguoiDung> dsBuoiHocNguoiDungTrongLop = await _buoiHocNguoiDungRepository.GetTheoDanhSachMaBuoi(danhSachBuoi);

                //Group lại theo mã người dùng sau đó dùng reducer để tính điểm 
                List<dynamic> lsDiemCuaNguoiDung = new List<dynamic>();

                foreach (var nguoiDung in dsBuoiHocNguoiDungTrongLop.GroupBy(x => x.MaNguoiDung))
                {
                    int tongDiem = 0;
                    foreach (var buoi in nguoiDung)
                    {
                        List<LichSuHocTap> lichSuHocTap = JsonConvert.DeserializeObject<List<LichSuHocTap>>(buoi.LichSuHocTap);

                        tongDiem += lichSuHocTap.Aggregate(0, (acc, x) => acc + x.Diem);

                    }
                    lsDiemCuaNguoiDung.Add(new { maNguoiDung = nguoiDung.Key, tongDiemNguoiDung = tongDiem });
                }

                //Có list danh sách người dùng và điểm rồi thì xử lý sort và lấy ra 3 người cao điểm nhất

                //Sort điểm người dùng
                List<dynamic> lsDiemCuaNguoiDungSorted = lsDiemCuaNguoiDung.OrderByDescending(nguoiDung => nguoiDung.tongDiemNguoiDung).Take(3).ToList();

                List<dynamic> lsIdNguoiDungCanLayRa = new List<dynamic>();

                foreach (var item in lsDiemCuaNguoiDungSorted)
                {
                    lsIdNguoiDungCanLayRa.Add(item.maNguoiDung);
                }

                //Thêm Ids của người dùng hiện tại vào list để trả ra cùng nếu như trong list chưa có
                if (!lsIdNguoiDungCanLayRa.Contains(maNguoiDung))
                {
                    lsIdNguoiDungCanLayRa.Add(maNguoiDung);
                }

                List<dynamic> thongTinChart = new List<dynamic>();

                //Duyệt theo mảng listId đã được sắp xếp để ra kết quả theo đúng thứ tự
                foreach (var (nguoiDung,index) in lsIdNguoiDungCanLayRa.Select((nguoiDung, index) => (nguoiDung, index)))
                {
                    //Lấy tên người dùng 
                    string tenNguoiDung = (await _nguoiDungRepository.GetSingleByIdAsync(nguoiDung)).HoTen;
                    //Lấy thứ tự người dùng
                    int thuTuNguoiDung = index + 1;

                    //Lọc lấy ra các dữ liệu của người dùng
                    IEnumerable<BuoiHoc_NguoiDung> dsBuoiHocCuaNguoiDungFiltered = dsBuoiHocNguoiDungTrongLop.Where(item => item.MaNguoiDung.Equals(nguoiDung));

                    //Xử lý tính điểm tổng của người dùng trong từng buổi học

                    List<dynamic> lsDiemCuaTungBuoiHoc = new List<dynamic>();

                    foreach (var item in dsBuoiHocCuaNguoiDungFiltered)
                    {
                        int thuTuBuoiHoc = (await _buoiHocRepository.GetSingleByIdAsync(item.MaBuoiHoc)).STT;
                        int tongDiemTrongBuoi = (JsonConvert.DeserializeObject<List<LichSuHocTap>>(item.LichSuHocTap)).Aggregate(0, (acc, x) => acc + x.Diem);

                        lsDiemCuaTungBuoiHoc.Add(new { sttBuoi = thuTuBuoiHoc, tongDiem = tongDiemTrongBuoi });
                    }

                    thongTinChart.Add(new { tenNguoiDung = tenNguoiDung, thuTuNguoiDung = thuTuNguoiDung, lsDiemCuaTungBuoiHoc = lsDiemCuaTungBuoiHoc });
                }
               

                //Lấy ra buổi học group theo skill
                foreach (var groupSkill in dsBuoiHoc.GroupBy(x => x.MaSkill))
                {
                    BuoiHocBySkillViewModel buoiHocBySkillVm = new BuoiHocBySkillViewModel();
                    List<BuoiHocViewModel> lsBuoiHocVm = new List<BuoiHocViewModel>();

                    buoiHocBySkillVm.tenSkill = groupSkill.Key;

                    buoiHocBySkillVm.DanhSachKhoaHocBySkill = new List<dynamic>();

                    //Lấy ra các khóa học video liên quan của skill
                    List<KeyValuePair<string, dynamic>> khoaHoccolums = new List<KeyValuePair<string, dynamic>>();

                    khoaHoccolums.Add(new KeyValuePair<string, dynamic>("MaSkill", groupSkill.Key));

                    IEnumerable<KhoaHoc> lsKhoaHocBySkill = await _khoaHocRepository.GetMultiByListConditionAndAsync(khoaHoccolums);

                    foreach (KhoaHoc khoaHoc in lsKhoaHocBySkill)
                    {
                        KhoaHocViewModel khoaHocVm = _mapper.Map<KhoaHocViewModel>(khoaHoc);

                        KhoaHocSkillViewModel khoaHocSkillVm = new KhoaHocSkillViewModel();

                        khoaHocSkillVm.TenKhoaHoc = khoaHocVm.TenKhoaHoc;
                        khoaHocSkillVm.HinhAnh = khoaHocVm.HinhAnh;
                        khoaHocSkillVm.SoNgayKichHoat = khoaHocVm.SoNgayKichHoat;
                        khoaHocSkillVm.DanhSachChuongHocSkill = new List<dynamic>();

                        //Lấy ra danh sách các chương học trong khóa
                        List<dynamic> dsChuongHocTrongKhoa = JsonConvert.DeserializeObject<List<dynamic>>(khoaHoc.DanhSachChuongHoc);

                        IEnumerable<ChuongHoc> lsChuongHoc = await _chuongHocRepository.GetMultiByIdAsync(dsChuongHocTrongKhoa);

                        //Duyệt từng chương học để lấy bài học 
                        foreach (ChuongHoc chuongHoc in lsChuongHoc)
                        {
                            
                            ChuongHocViewModel chuongHocVm = _mapper.Map<ChuongHocViewModel>(chuongHoc);

                            chuongHocVm.DanhSachBaiHoc = new List<dynamic>();

                            //Lấy ra danh sách bài học trong chương
                            List<dynamic> dsBaiHocTrongChuong = JsonConvert.DeserializeObject<List<dynamic>>(chuongHoc.DanhSachBaiHoc);
                            IEnumerable<BaiHoc> lsBaiHoc = await _baiHocRepository.GetMultiByIdAsync(dsBaiHocTrongChuong);

                            //Chuyển list modal bài học thành list view

                            List<BaiHocViewModel> lsBaiHocVm = _mapper.Map<List<BaiHocViewModel>>(lsBaiHoc);

                            chuongHocVm.DanhSachBaiHoc.Add(lsBaiHocVm);
                            khoaHocSkillVm.DanhSachChuongHocSkill.Add(chuongHocVm);
                        }
                        
                        buoiHocBySkillVm.DanhSachKhoaHocBySkill.Add(khoaHocSkillVm);

                    }



                    //Duyệt từng buổi học trong skill để lấy data bài học

                    foreach (var buoiHoc in groupSkill)
                    {
           
                        List<dynamic> dsBaiHocTrongBuoi = JsonConvert.DeserializeObject<List<dynamic>>(buoiHoc.DanhSachBaiHocTracNghiem);

                        BuoiHocViewModel buoiHocVm = _mapper.Map<BuoiHocViewModel>(buoiHoc);

                        //Lấy ra dữ liệu BuoiHoc_NguoiDung về bài tập
                        List<KeyValuePair<string, dynamic>> buoiHocNguoiDungColumns = new List<KeyValuePair<string, dynamic>>();
                        //Gán cứng mã người dùng của sĩ để test sau này sẽ lấy từ token gắn ở header
                        buoiHocNguoiDungColumns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", maNguoiDung));
                        buoiHocNguoiDungColumns.Add(new KeyValuePair<string, dynamic>("MaBuoiHoc", buoiHoc.Id));

                        BuoiHoc_NguoiDung buoiHocNguoiDung = await _buoiHocNguoiDungRepository.GetSingleByListConditionAsync(buoiHocNguoiDungColumns);

                        if (buoiHocNguoiDung != null)
                        {
                            IEnumerable<LichSuHocTap> lsLichSuHocTap = JsonConvert.DeserializeObject<IEnumerable<LichSuHocTap>>(buoiHocNguoiDung.LichSuHocTap);

                            buoiHocVm.LichSuHocTap = _mapper.Map<List<LichSuHocTapViewModel>>(lsLichSuHocTap);
                        }

                        //Lấy ra dữ liệu của các View và gán cho buoiHocView
                        //TaiLieuBaiHoc
                        IEnumerable <TaiLieuBaiHoc> lsTaiLieuBaiHoc = await _taiLieuBaiHocRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TaiLieuBaiHoc = _mapper.Map<List<TaiLieuBaiHocViewModel>>(lsTaiLieuBaiHoc);

                        //TaiLieuBaiTap
                        IEnumerable<TaiLieuBaiTap> lsTaiLieuBaiTap = await _taiLieuBaiTapRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TaiLieuBaiTap = (_mapper.Map<List<TaiLieuBaiTapViewModel>>(lsTaiLieuBaiTap));

                        //TaiLieuDocThem
                        IEnumerable<TaiLieuDocThem> lsTaiLieuDocThem = await _taiLieuDocThemRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TaiLieuDocThem = (_mapper.Map<List<TaiLieuDocThemViewModel>>(lsTaiLieuDocThem));

                        //TaiLieuProjectLamThem
                        IEnumerable<TaiLieuProjectLamThem> lsTaiLieuProjectLamThem = await _taiLieuProjectLamThemRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TaiLieuProjectLamThem = (_mapper.Map<List<TaiLieuProjectLamThemViewModel>>(lsTaiLieuProjectLamThem));

                        //TaiLieuCapstone
                        IEnumerable<TaiLieuCapstone> lsTaiLieuCapstone = await _taiLieuCapstoneRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TaiLieuCapstone = (_mapper.Map<List<TaiLieuCapstoneViewModel>>(lsTaiLieuCapstone));

                        //TracNghiem
                        IEnumerable<TracNghiem> lsTracNghiem = await _tracNghiemRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TracNghiem = (_mapper.Map<List<TracNghiemViewModel>>(lsTracNghiem));


                        //Add VideoXemLai
                        List<KeyValuePair<string, dynamic>> colums = new List<KeyValuePair<string, dynamic>>();

                        colums.Add(new KeyValuePair<string, dynamic>("MaBuoi", buoiHoc.Id));

                        IEnumerable<XemLaiBuoiHoc> lsXemLaiBuoiHoc = await _xemLaiBuoiHocRepository.GetMultiByListConditionAndAsync(colums);

                        if (lsXemLaiBuoiHoc != null)
                        {
                            foreach (XemLaiBuoiHoc video in lsXemLaiBuoiHoc)
                            {
                                XemLaiBuoiHocViewModel xemLaiBuoiHocVm = new XemLaiBuoiHocViewModel();
                                xemLaiBuoiHocVm = _mapper.Map<XemLaiBuoiHocViewModel>(video);
                                buoiHocVm.VideoXemLai.Add(xemLaiBuoiHocVm);
                            }
                        }

                        //Add các Video extra vào
                        IEnumerable<VideoExtra> lsVideoExtra = await _videoExtraRepository.GetMultiByListConditionAndAsync(colums);

                        if (lsVideoExtra != null)
                        {
                            foreach (VideoExtra video in lsVideoExtra)
                            {
                                VideoExtraViewModel videoExtraVm = new VideoExtraViewModel();
                                videoExtraVm = _mapper.Map<VideoExtraViewModel>(video);
                                buoiHocVm.VideoExtra.Add(videoExtraVm);
                            }
                        }
                        //Add buổi học vào list buổi học
                        lsBuoiHocVm.Add(buoiHocVm);
                    }
                    
                    buoiHocBySkillVm.DanhSachBuoiHoc = lsBuoiHocVm;
                    dsBuoiHocBySkillVm.Add(buoiHocBySkillVm);
                }
               
                return new ResponseEntity(StatusCodeConstants.OK, new { thongBao = lsNoiDungThongBaoVm, thongKeDiemNguoiDung = thongTinChart, thongTinLopHoc = thongTinLopHoc, danhSachBuoiHocTheoSkill = dsBuoiHocBySkillVm });
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> AddClassesToClass(int classId, int classesId) 
        {
        
                try
                {
                //Kiểm tra buổi học có trong hệ thống hay không
                BuoiHoc buoiHoc = await _buoiHocRepository.GetSingleByIdAsync(classesId);
                if (buoiHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }
 
                    //Lấy ra danh sách các buổi học của lớp
                    LopHoc lopHocHienTai = await _lopHocRepository.GetSingleByIdAsync(classId);

                    List<int> lsCacBuoiHoc = JsonConvert.DeserializeObject<List<int>>(lopHocHienTai.DanhSachBuoi);
                //Kiểm tra xem hiện tại có chứa buổi học muốn thêm chưa
             
                if (lsCacBuoiHoc.Contains(classesId))
                    {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Buổi học đã tồn tại", MessageConstants.INSERT_ERROR);
                    }

                    lsCacBuoiHoc.Add(classesId);

                    string lsCacBuoiHocString = JsonConvert.SerializeObject(lsCacBuoiHoc);

                    lopHocHienTai.DanhSachBuoi = lsCacBuoiHocString; 

 
                    if (( await _lopHocRepository.UpdateAsync(classId, lopHocHienTai)) == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, lopHocHienTai, MessageConstants.INSERT_ERROR);


                return new ResponseEntity(StatusCodeConstants.OK, lopHocHienTai, MessageConstants.INSERT_SUCCESS);
                }
                catch (Exception ex)
                {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
                }

        }
    }
}