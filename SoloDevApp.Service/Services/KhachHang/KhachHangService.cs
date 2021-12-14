using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using SoloDevApp.Service.ViewModels.KhachHang;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IKhachHangService : IService<KhachHang, KhachHangViewModel>
    {
        Task<ResponseEntity> RegisterAsync(int id, KhachHangGhiDanhViewModel modelVm);
        Task<ResponseEntity> GenerateTokenAsync(int id);
        Task<ResponseEntity> CheckTokenAsync(string token);
        Task<ResponseEntity> UpdateInfoAsync(int id, ThongTinKHViewModel modelVm);
        Task<ResponseEntity> RemoveCustomerByClassAsync(NguoiDung_LopHocViewModel modelVm);
        Task<ResponseEntity> GetAllCustomer(int page, int size, string keywords, string filter);

    }

    public class KhachHangService : ServiceBase<KhachHang, KhachHangViewModel>, IKhachHangService
    {
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly ILopHocRepository _lopHocRepository;
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly IHocPhiRepository _hocPhiRepository;
        private readonly IAppSettings _appSettings;

        public KhachHangService(IKhachHangRepository khachHangRepository,
            ILopHocRepository lopHocRepository,
            INguoiDungRepository nguoiDungRepository,
            IHocPhiRepository hocPhiRepository,
            IAppSettings appSettings,
            IMapper mapper)
            : base(khachHangRepository, mapper)
        {
            _lopHocRepository = lopHocRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _khachHangRepository = khachHangRepository;
            _hocPhiRepository = hocPhiRepository;
            _appSettings = appSettings;
        }

        public override async Task<ResponseEntity> InsertAsync(KhachHangViewModel modelVm)
        {
            try
            {
                KhachHang entity = _mapper.Map<KhachHang>(modelVm);
                entity.BiDanh = FuncUtilities.BestLower(entity.BiDanh);

                IEnumerable<KhachHang> checkKH = await _khachHangRepository.GetTheoEmailDienThoai(modelVm.ThongTinKH.Email);

                if (modelVm.ThongTinKH.Email.Trim() != "" && modelVm.ThongTinKH.Email != null)
                {
                    if (checkKH.Count() > 0)
                    {
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "0");

                    }
                }

                if (modelVm.ThongTinKH.SoDienThoai.Trim() != "" && modelVm.ThongTinKH.SoDienThoai != null)
                {
                    checkKH = await _khachHangRepository.GetTheoEmailDienThoai(modelVm.ThongTinKH.SoDienThoai);
                    if (checkKH.Count() > 0)
                    {
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "0");

                    }
                }
              


                entity = await _khachHangRepository.InsertAsync(entity);

                HocPhiViewModel hocPhiVm = new HocPhiViewModel();
                if (modelVm.MaTrangThaiKH == 2)
                {
                    hocPhiVm = modelVm.HocPhi;
                    hocPhiVm.MaKH = entity.Id.ToString();

                    HocPhi hocPhi = _mapper.Map<HocPhi>(hocPhiVm);
                    await _hocPhiRepository.InsertAsync(hocPhi);
                }

                modelVm = _mapper.Map<KhachHangViewModel>(entity);
                modelVm.HocPhi = hocPhiVm;
                return new ResponseEntity(StatusCodeConstants.CREATED, modelVm, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> UpdateAsync(dynamic id, KhachHangViewModel modelVm)
        {
            try
            {
                string sEmailOld = "";

                KhachHang khachHang = await _khachHangRepository.GetSingleByIdAsync(id);
                if (khachHang == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, modelVm);

                IEnumerable<KhachHang> checkKH = await _khachHangRepository.GetTheoEmailDienThoai(modelVm.ThongTinKH.Email);
                checkKH = checkKH.Where(n => n.Id != id);

                if (modelVm.ThongTinKH.Email.Trim() != "" && modelVm.ThongTinKH.Email != null)
                {
                   
                    if (checkKH.Count() > 0)
                    {
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "0");

                    }
                }

                if (modelVm.ThongTinKH.SoDienThoai.Trim() != "" && modelVm.ThongTinKH.SoDienThoai != null)
                {
                    checkKH = await _khachHangRepository.GetTheoEmailDienThoai(modelVm.ThongTinKH.SoDienThoai);
                    checkKH = checkKH.Where(n => n.Id != id);

                    if (checkKH.Count() > 0)
                    {
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "0");

                    }
                }

                KhachHangViewModel khachHangVm = _mapper.Map<KhachHangViewModel>(khachHang);
                khachHangVm.ThongTinKH = JsonConvert.DeserializeObject<ThongTinKHViewModel>(khachHang.ThongTinKH);

                sEmailOld = khachHangVm.ThongTinKH.Email;

                // CẬP NHẬT THÔNG TIN KHÁCH HÀNG
                khachHangVm.TenKH = modelVm.TenKH;
                khachHangVm.BiDanh = modelVm.BiDanh;
                khachHangVm.ThongTinKH.Email = modelVm.ThongTinKH.Email;
                khachHangVm.ThongTinKH.SoDienThoai = modelVm.ThongTinKH.SoDienThoai;
                khachHangVm.ThongTinKH.NguonGioiThieu = modelVm.ThongTinKH.NguonGioiThieu;
                khachHangVm.ThongTinKH.CongViecHienTai = modelVm.ThongTinKH.CongViecHienTai;
                khachHangVm.ThongTinKH.TruongDaVaDangHoc = modelVm.ThongTinKH.TruongDaVaDangHoc;
                khachHangVm.ThongTinKH.MucTieu = modelVm.ThongTinKH.MucTieu;
                khachHangVm.ThongTinKH.DiemTiemNang = modelVm.ThongTinKH.DiemTiemNang;
                khachHangVm.ThongTinKH.CmndTruoc = modelVm.ThongTinKH.CmndTruoc;
                khachHangVm.ThongTinKH.CmndSau = modelVm.ThongTinKH.CmndSau;
                khachHangVm.MaTrangThaiKH = modelVm.MaTrangThaiKH;
                khachHangVm.NgayNhacLai = modelVm.NgayNhacLai;
                khachHangVm.LinkFacebook = modelVm.LinkFacebook;
                khachHangVm.LichSuGoiVaGhiChu = modelVm.LichSuGoiVaGhiChu;
                khachHang = _mapper.Map<KhachHang>(khachHangVm);
                khachHang.DaNhapForm = true;
                khachHang.NuocNgoai = modelVm.NuocNgoai;
                await _khachHangRepository.UpdateAsync(id, khachHang);


                // CẬP NHẬT THÔNG TIN NGƯỜI DÙNG
                NguoiDung nguoiDung = await _nguoiDungRepository.GetByEmailAsync(sEmailOld);
                if (nguoiDung != null)
                {
                    nguoiDung.HoTen = modelVm.TenKH;
                    nguoiDung.BiDanh = modelVm.BiDanh;
                    nguoiDung.SoDT = modelVm.ThongTinKH.SoDienThoai;
                    nguoiDung.Email = modelVm.ThongTinKH.Email;
                    nguoiDung.NuocNgoai = modelVm.NuocNgoai;

                    await _nguoiDungRepository.UpdateAsync(nguoiDung.Id, nguoiDung);
                }

                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> CheckTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jsonToken = handler.ReadJwtToken(token).Payload;
                var dateEXP = FuncUtilities.ConvertToTimeStamp((int)jsonToken.Exp);
                var dateNow = FuncUtilities.ConvertStringToDate();
                if (dateEXP < dateNow)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST);
                return new ResponseEntity(StatusCodeConstants.OK, jsonToken);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GenerateTokenAsync(int id)
        {
            try
            {
                KhachHang khachHang = await _khachHangRepository.GetSingleByIdAsync(id);
                if (khachHang == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                string token = GenerateToken(khachHang);
                return new ResponseEntity(StatusCodeConstants.CREATED, token);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> RegisterAsync(int id, KhachHangGhiDanhViewModel modelVm)
        {
            try
            {
                KhachHang khachHang = await _khachHangRepository.GetSingleByIdAsync(id);
                if (khachHang == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                if(khachHang.DaNhapForm==false)
                    return new ResponseEntity(StatusCodeConstants.OK,"0");

                DangKyViewModel dangKyModel = new DangKyViewModel()
                {
                    Email = modelVm.Email,
                    MatKhau = modelVm.MatKhau,
                    HoTen = modelVm.HoTen,
                    BiDanh = modelVm.BiDanh,
                    SoDT = modelVm.SoDT,
                    Avatar = "/static/user-icon.png"
                };

                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                // Tạo tài khoản cho khách hàng nếu chưa có
                if (entity == null)
                {
                    entity = _mapper.Map<NguoiDung>(dangKyModel);
                    entity.Id = Guid.NewGuid().ToString();
                    // Mã hóa mật khẩu
                    entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(modelVm.MatKhau);
                    entity.MaNhomQuyen = "HOCVIEN";
                    entity.BiDanh = FuncUtilities.BestLower(entity.HoTen);
                    //entity.TenKH = entity.HoTen;
                    entity.DanhSachLopHoc = "[]";
                    entity.NgayTao = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                   
                    await _nguoiDungRepository.InsertAsync(entity);
                }
                // Lấy ra lớp học có id trùng với mã lớp học truyền lên
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLopHoc);

                //LopHocViewModel lopHocVm = _mapper.Map<LopHocViewModel>(lopHoc);
                // Thêm vào danh sách
                HashSet<string> dsHocVien = JsonConvert.DeserializeObject<HashSet<string>>(lopHoc.DanhSachHocVien);
                dsHocVien.Add(entity.Id);
                //Thêm vào danh sách backup
                // Thêm vào danh sách
                List<ThongTinHocVienGhiDanh> dsHocVienNew = JsonConvert.DeserializeObject<List<ThongTinHocVienGhiDanh>>(lopHoc.DanhSachHocVienNew);
                //dsHocVien.Add(entity.Id);
                dsHocVienNew.Add(new ThongTinHocVienGhiDanh { Id = entity.Id, ngayGhiDanh = DateTime.Now });

                lopHoc.DanhSachHocVien = JsonConvert.SerializeObject(dsHocVien);
                lopHoc.DanhSachHocVienNew = JsonConvert.SerializeObject(dsHocVienNew);

                // Cập nhật lại thông tin lớp
                await _lopHocRepository.UpdateAsync(lopHoc.Id, lopHoc);

                // Cập nhật lại danh sách lớp học cho người dùng
                HashSet<string> dsLopHoc = new HashSet<string>();
                if (!string.IsNullOrEmpty(entity.DanhSachLopHoc))
                {
                    dsLopHoc = JsonConvert.DeserializeObject<HashSet<string>>(entity.DanhSachLopHoc);
                }
                dsLopHoc.Add(lopHoc.Id.ToString());
                entity.DanhSachLopHoc = JsonConvert.SerializeObject(dsLopHoc);

                await _nguoiDungRepository.UpdateAsyncHasArrayNull(entity.Id, entity);

                return new ResponseEntity(StatusCodeConstants.CREATED, lopHoc, MessageConstants.SIGNUP_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> UpdateInfoAsync(int id, ThongTinKHViewModel modelVm)
        {
            try
            {

                IEnumerable<KhachHang> checkKH = await _khachHangRepository.GetTheoEmailDienThoai(modelVm.Email);
                checkKH = checkKH.Where(n => n.Id != id);

                if (modelVm.Email.Trim() != "" && modelVm.Email != null)
                {

                    if (checkKH.Count() > 0)
                    {
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "0");

                    }
                }

                if (modelVm.SoDienThoai.Trim() != "" && modelVm.SoDienThoai != null)
                {
                    checkKH = await _khachHangRepository.GetTheoEmailDienThoai(modelVm.SoDienThoai);
                    checkKH = checkKH.Where(n => n.Id != id);

                    if (checkKH.Count() > 0)
                    {
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "0");

                    }
                }

                var khachHang = await _khachHangRepository.GetSingleByIdAsync(id);
                var khachHangVm = _mapper.Map<KhachHangViewModel>(khachHang);

                var thongTinKhacHang = khachHangVm.ThongTinKH;
               
                modelVm.MucTieu = thongTinKhacHang.MucTieu;
                modelVm.GhiChu = thongTinKhacHang.GhiChu;
                modelVm.TiengAnh = thongTinKhacHang.TiengAnh;
                modelVm.MaCaNhan = thongTinKhacHang.MaCaNhan;

                khachHangVm.ThongTinKH = modelVm;

                khachHang = _mapper.Map<KhachHang>(khachHangVm);
                khachHang.DaNhapForm = true;
                khachHang.BiDanh = FuncUtilities.BestLower(khachHang.TenKH);
                
                await _khachHangRepository.UpdateAsync(id, khachHang);


                ThongTinKHViewModel thongTinKHVm = JsonConvert.DeserializeObject<ThongTinKHViewModel>(khachHang.ThongTinKH);
                // Tạo tài khoản mới cho user
                NguoiDung entity = new NguoiDung();
                entity.Id = Guid.NewGuid().ToString();
                entity.Email = thongTinKHVm.Email;
                entity.MatKhau = BCrypt.Net.BCrypt.HashPassword("Cybersoft@123");
                entity.HoTen = khachHang.TenKH;
                entity.BiDanh = khachHang.BiDanh;
                entity.SoDT = thongTinKHVm.SoDienThoai;
                entity.Avatar = "/static/user-icon.png";
                entity.MaNhomQuyen = "HOCVIEN";
                entity.DanhSachLopHoc = "[]";
                entity.NgayTao = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");

                // Thực hiện truy vấn thêm mới
                await _nguoiDungRepository.InsertAsync(entity);

                return new ResponseEntity(StatusCodeConstants.OK, null, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        private string GenerateToken(KhachHang khachHang)
        {
            var arrInfo = new List<Claim> {
                new Claim("MaKhachHang", khachHang.Id.ToString())
            };

            SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Secret));
            var token = new JwtSecurityToken(
                        claims: arrInfo,
                        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                        expires: new DateTimeOffset(DateTime.Now.AddDays(3)).DateTime,
                        signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
                );

            //string token1 = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<ResponseEntity> RemoveCustomerByClassAsync(NguoiDung_LopHocViewModel modelVm)
        {
            //Lấy danh sách lớp học 
            var nguoiDung = await _nguoiDungRepository.GetSingleByIdAsync(modelVm.idNguoiDung);
            HashSet<string> dsLopHoc = JsonConvert.DeserializeObject<HashSet<string>>(nguoiDung.DanhSachLopHoc);
            dsLopHoc.Remove(modelVm.idLopHoc);

            if (dsLopHoc.Count() == 0) dsLopHoc.Add("0");

            nguoiDung.DanhSachLopHoc =  JsonConvert.SerializeObject(dsLopHoc);
            //Cập nhật lại khách hàng 
            await _nguoiDungRepository.UpdateAsync(nguoiDung.Id, nguoiDung);


            //Lấy danh sách khách hàng

            var lopHoc = await _lopHocRepository.GetSingleByIdAsync(modelVm.idLopHoc);
            HashSet<string> dsHocVien = JsonConvert.DeserializeObject<HashSet<string>>(lopHoc.DanhSachHocVien);
            dsHocVien.Remove(modelVm.idNguoiDung);

            List<ThongTinHocVienGhiDanh> dsHVGD = JsonConvert.DeserializeObject<List<ThongTinHocVienGhiDanh>>(lopHoc.DanhSachHocVienNew);
            var hv = dsHVGD.Where(n => n.Id == nguoiDung.Id).ToList();
            if (hv.Count() > 0)
            {
                foreach(var item in hv)
                {
                    dsHVGD.Remove(item);
                }
            }

            lopHoc.DanhSachHocVienNew = JsonConvert.SerializeObject(dsHVGD);
            lopHoc.DanhSachHocVien = JsonConvert.SerializeObject(dsHocVien); 
            await _lopHocRepository.UpdateAsyncHasArrayNull(lopHoc.Id, lopHoc);



            //Cập nhật lại lớp học
            return new ResponseEntity(StatusCodeConstants.OK, null, MessageConstants.UPDATE_SUCCESS);

        }

        public async Task<ResponseEntity> GetAllCustomer(int page, int size, string keywords, string filter)
        {
            var dsKhachHang = await _khachHangRepository.GetPagingAsync(page, size, keywords, filter);


            List<KhachHangViewDetailModel> dsKHDetail = new List<KhachHangViewDetailModel>();

            foreach (var kh in dsKhachHang.Items)
            {

                KhachHangViewDetailModel khDetail = new KhachHangViewDetailModel();
                khDetail.Id = kh.Id;
                khDetail.TenKH = kh.TenKH;
                khDetail.BiDanh = kh.BiDanh;
                khDetail.DiaChi = JsonConvert.DeserializeObject<DiaChiViewModel>(kh.DiaChi);
                khDetail.ThongTinKH = JsonConvert.DeserializeObject<ThongTinKHViewModel>(kh.ThongTinKH);
                khDetail.MaTrangThaiKH = kh.MaTrangThaiKH;
                khDetail.MaDoiTacGioiThieu = kh.MaDoiTacGioiThieu;
                khDetail.DanhSachNguoiTuVan = kh.DanhSachNguoiTuVan;
                khDetail.LichSuGoiVaGhiChu = kh.LichSuGoiVaGhiChu;
                khDetail.MaNguoiTuVan = kh.MaNguoiTuVan;
                khDetail.MaNguonGioiThieu = kh.MaNguonGioiThieu;
                khDetail.NgayTao = kh.NgayTao;
                khDetail.LinkFacebook = kh.LinkFacebook;
                khDetail.NgayNhacLai = kh.NgayNhacLai;
                khDetail.NuocNgoai = kh.NuocNgoai;

                ThongTinKHViewModel ttkh = JsonConvert.DeserializeObject<ThongTinKHViewModel>(kh.ThongTinKH);
                List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
                columns.Add(new KeyValuePair<string, dynamic>("Email", ttkh.Email));

                NguoiDung nd = await _nguoiDungRepository.GetByEmailAsync(ttkh.Email);

                if (nd != null)
                {
                    if (nd.DanhSachLopHoc != "[]" && nd.DanhSachLopHoc != null && nd.DanhSachLopHoc != "" &&nd.Email.Trim()!="")
                    {
                        List<string> lopHoc = JsonConvert.DeserializeObject<List<string>>(nd.DanhSachLopHoc);
                        List<dynamic> lstLopHoc = new List<dynamic>();
                        foreach (var id in lopHoc)
                        {
                            lstLopHoc.Add(int.Parse(id));
                        }
                        var lstLopHocDetail = await _lopHocRepository.GetMultiByListIdAsync(lstLopHoc);
                        khDetail.thongTinLopHoc = lstLopHocDetail;

                    }
                }
                dsKHDetail.Add(khDetail);
            }
            var modelVm = new PagingResult<KhachHangViewDetailModel>();
            modelVm.Items = dsKHDetail;
            modelVm.PageIndex = page;
            modelVm.PageSize = size;
            modelVm.Keywords = keywords;
            modelVm.TotalRow = dsKhachHang.TotalRow;
            return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.INSERT_SUCCESS);

        }
    }
}