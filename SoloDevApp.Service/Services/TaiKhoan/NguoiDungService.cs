using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SoloDevApp.Service.Utilities;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace SoloDevApp.Service.Services
{
    public interface INguoiDungService : IService<NguoiDung, NguoiDungViewModel>
    {
        Task<ResponseEntity> SignInAsync(DangNhapViewModel modelVm);

        Task<ResponseEntity> SignInFacebookAsync(DangNhapFacebookViewModel modelVm);

        Task<ResponseEntity> SignUpAsync(DangKyViewModel modelVm);

        Task<ResponseEntity> InsertUserAsync(DangKyViewModel modelVm);

        Task<ResponseEntity> UpdateUserAsync(string id, SuaNguoiDungViewModel modelVm);

        Task<ResponseEntity> ChangePasswordAsync(DoiMatKhauViewModel modelVm);

        Task<ResponseEntity> GetByRoleGroupAsync(string column, List<dynamic> values);

        Task<ResponseEntity> CheckPass(NguoiDung model);

        Task<ResponseEntity> ExportExel();

    }

    public class NguoiDungService : ServiceBase<NguoiDung, NguoiDungViewModel>, INguoiDungService
    {
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly INhomQuyenRepository _nhomQuyenRepository;
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly ILopHocRepository _lopHocRepository;
        private readonly HttpClient _httpClient;
        private readonly IAppSettings _appSettings;

        public NguoiDungService(INguoiDungRepository nguoiDungRepository,
            IMapper mapper, INhomQuyenRepository nhomQuyenRepository,
            IKhachHangRepository khachHangRepository,
            ILopHocRepository lopHocRepository,
            IAppSettings appSettings)
            : base(nguoiDungRepository, mapper)
        {
            _nguoiDungRepository = nguoiDungRepository;
            _nhomQuyenRepository = nhomQuyenRepository;
            _khachHangRepository = khachHangRepository;
            _lopHocRepository = lopHocRepository;
            _appSettings = appSettings;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ResponseEntity> ExportExel()
        {
            try
            {
                DataTable dt = new DataTable("Report");
                dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Name"),
                                                     new DataColumn("Email"),
                                                     new DataColumn("Phone")});

                IEnumerable<NguoiDung> nguoiDung = await _nguoiDungRepository.GetAllAsync();

                foreach (var item in nguoiDung)
                {
                    dt.Rows.Add(item.HoTen, item.Email, item.SoDT);
                }

                var folderName = Path.Combine("wwwroot", "files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string fileName = $"test.xlsx";

                // Tạo đường dẫn tới file
                string path = Path.Combine(pathToSave, fileName);

                using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
                {

                    wb.Worksheets.Add(dt);
                    wb.SaveAs(path);

                }
                return new ResponseEntity(StatusCodeConstants.OK, "1");

            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }


        public async Task<ResponseEntity> CheckPass(NguoiDung model)
        {
            try
            {
                NguoiDung entity = await _nguoiDungRepository.GetSingleByIdAsync(model.Id);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                if(model.Urls != entity.MatKhau)
                    return new ResponseEntity(StatusCodeConstants.OK, "0");

                NguoiDungViewModel nguoiDung = _mapper.Map<NguoiDungViewModel>(entity);

                return new ResponseEntity(StatusCodeConstants.OK, nguoiDung, MessageConstants.UPDATE_SUCCESS);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public async Task<ResponseEntity> ChangePasswordAsync(DoiMatKhauViewModel modelVm)
        {
            try
            {
                NguoiDung entity = await _nguoiDungRepository.GetSingleByIdAsync(modelVm.Id);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(modelVm.MatKhau);

                entity = await _nguoiDungRepository.UpdateAsync(entity.Id, entity);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.UPDATE_ERROR);

                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public async Task<ResponseEntity> GetByRoleGroupAsync(string column, List<dynamic> values)
        {
            try
            {
                var columns = new List<KeyValuePair<string, dynamic>>();
                foreach (string value in values)
                {
                    columns.Add(new KeyValuePair<string, dynamic>(column, value));
                }

                IEnumerable<NguoiDung> entities = await _nguoiDungRepository.GetMultiByListConditionAsync(columns);
                List<NguoiDungViewModel> modelVm = _mapper.Map<List<NguoiDungViewModel>>(entities);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> SignInAsync(DangNhapViewModel modelVm)
        {
            try
            {
                // Lấy ra thông tin người dùng từ database dựa vào email
                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                if (entity == null)// Nếu email sai
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, modelVm, MessageConstants.SIGNIN_WRONG);
                // Kiểm tra mật khẩu có khớp không
                if (!BCrypt.Net.BCrypt.Verify(modelVm.MatKhau, entity.MatKhau))
                    // Nếu password không khớp
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, modelVm, MessageConstants.SIGNIN_WRONG);
                // Tạo token
                string token = await GenerateToken(entity);
                if (token == string.Empty)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.TOKEN_GENERATE_ERROR);

                NguoiDung updateModel = entity;
                updateModel.Urls = entity.MatKhau;
                await _nguoiDungRepository.UpdateAsync(updateModel.Id, updateModel);


                entity.Token = token;

              
                NguoiDungViewModel model = _mapper.Map<NguoiDungViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstants.SIGNIN_SUCCESS);
            }
            catch(Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.SIGNIN_ERROR);
            }
        }

        public async Task<ResponseEntity> SignInFacebookAsync(DangNhapFacebookViewModel modelVm)
        {
            string[] ERR_MESSAGE = { "Email bị trống, kiểm tra lại facebook!", "Email này đã được sử dụng cho tài khoản facebook khác!", "Email không chính xác!" };
            string[] ERR_STATUS = { "EMAIL_NULL", "EMAIL_EXISTS", "EMAIL_INCORRECT" };

            try
            {
                await _lopHocRepository.EnableAsync();
                await _lopHocRepository.DisableAsync();

              

                // Nếu facebook id sai và email chưa nhập
                if (string.IsNullOrEmpty(modelVm.Email))
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ERR_STATUS[0], ERR_MESSAGE[0]);

                // Lấy ra thông tin người dùng từ database dựa vào email
                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);

                if (entity == null)
                {

                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ERR_STATUS[2], ERR_MESSAGE[2]);
                }
                else
                {
                    // Lưu FacebookId vào database
                    entity.FacebookId = modelVm.FacebookId;
                    entity = await _nguoiDungRepository.UpdateAsync(entity.Id, entity);
                }

               
                // Tạo token
                entity.Token = await GenerateToken(entity);
                NguoiDungViewModel result = _mapper.Map<NguoiDungViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.SIGNIN_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.SIGNIN_ERROR);
            }
        }

        public async Task<ResponseEntity> SignUpAsync(DangKyViewModel modelVm)
        {
            try
            {
                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                if (entity != null) // Kiểm tra email đã được sử dụng bởi tài khoản khác chưa
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.EMAIL_EXITST);

                entity = _mapper.Map<NguoiDung>(modelVm);

                entity.Id = Guid.NewGuid().ToString();
                // Mã hóa mật khẩu
                entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(modelVm.MatKhau);
                entity.Avatar = !string.IsNullOrEmpty(modelVm.Avatar) ? modelVm.Avatar : "/static/user-icon.png";
                entity.MaNhomQuyen = "HOCVIEN";

                entity = await _nguoiDungRepository.InsertAsync(entity);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.SIGNUP_ERROR);

                NguoiDungViewModel model = _mapper.Map<NguoiDungViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.CREATED, model, MessageConstants.SIGNUP_SUCCESS);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.SIGNUP_ERROR);
            }
        }

        public async Task<ResponseEntity> InsertUserAsync(DangKyViewModel modelVm)
        {
            try
            {

                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                if (entity != null) // Kiểm tra email đã được sử dụng bởi tài khoản khác chưa
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.EMAIL_EXITST);

                entity = _mapper.Map<NguoiDung>(modelVm);
                entity.BiDanh = FuncUtilities.BestLower(entity.HoTen);
                entity.Id = Guid.NewGuid().ToString();
                entity.DanhSachLopHoc = "[]";
                entity.ThongTinMoRong = "[]";
                // Mã hóa mật khẩu
                entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(modelVm.MatKhau);
                entity.Avatar = !string.IsNullOrEmpty(modelVm.Avatar) ? modelVm.Avatar : "/static/user-icon.png";

                entity = await _nguoiDungRepository.InsertAsync(entity);

                NguoiDungViewModel model = _mapper.Map<NguoiDungViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.CREATED, model, MessageConstants.SIGNUP_SUCCESS);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.SIGNUP_ERROR);
            }
        }

        public async Task<ResponseEntity> UpdateUserAsync(string id, SuaNguoiDungViewModel modelVm)
        {
            try
            {
                // CẬP NHẬT THÔNG TIN NGƯỜI DÙNG
                NguoiDung entity = await _nguoiDungRepository.GetSingleByIdAsync(id);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, modelVm);

                entity = _mapper.Map<NguoiDung>(modelVm);
                await _nguoiDungRepository.UpdateAsync(id, entity);

                // CẬP NHẬT THÔNG TIN KHÁCH HÀNG
                KhachHang khachHang = await _khachHangRepository.GetByEmailAsync(entity.Email);
                if (khachHang!=null)
                {
                    KhachHangViewModel khachHangVm = _mapper.Map<KhachHangViewModel>(khachHang);
                    khachHangVm.TenKH = entity.HoTen;
                    khachHangVm.ThongTinKH.Email = entity.Email;
                    khachHangVm.ThongTinKH.SoDienThoai = entity.SoDT;

                    khachHang = _mapper.Map<KhachHang>(khachHangVm);
                    khachHang.DaNhapForm = true;
                    await _khachHangRepository.UpdateAsync(khachHang.Id, khachHang);
                }
            

                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> GetSingleByIdAsync(dynamic id)
        {
            try
            {
                var entity = await _nguoiDungRepository.GetSingleByIdAsync(id);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                NguoiDungViewModel modelVm = _mapper.Map<NguoiDungViewModel>(entity);
                if(modelVm.DanhSachLopHoc != null)
                {
                    List<dynamic> dsMaLopHoc = JsonConvert.DeserializeObject<List<dynamic>>(modelVm.DanhSachLopHoc);
                    modelVm.ThongTinLopHoc = (await _lopHocRepository.GetMultiByListIdAsync(dsMaLopHoc)).ToList();
                }
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        private async Task<string> GenerateToken(NguoiDung entity)
        {
            try
            {
                NhomQuyen nhomQuyen = await _nhomQuyenRepository.GetSingleByIdAsync(entity.MaNhomQuyen);
                if (nhomQuyen == null)
                    return string.Empty;
                List<string> roles = JsonConvert.DeserializeObject<List<string>>(nhomQuyen.DanhSachQuyen);
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, entity.Id));
                claims.Add(new Claim(ClaimTypes.Email, entity.Email));
                foreach (var item in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item.Trim()));
                }

                var secret = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var token = new JwtSecurityToken(
                        claims: claims,
                        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                        expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}