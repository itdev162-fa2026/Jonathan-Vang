const API_BASE_URL = 'http://localhost:5235';

export const getProducts = async () => {
  const res = await fetch(`${API_BASE_URL}/products`);
  if (!res.ok) throw new Error('Failed to fetch products');
  return res.json();
};

export const getProductById = async (id) => {
  const res = await fetch(`${API_BASE_URL}/products/${id}`);
  if (!res.ok) throw new Error('Failed to fetch product');
  return res.json();
};
